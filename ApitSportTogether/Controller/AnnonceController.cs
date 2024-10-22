using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.ObjectContext;
using ApiSportTogether.model.ObjectVue;
using ApiSportTogether.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiSportTogether.Controller
{
    [ApiController]
    [Route("ApiSportTogether/[controller]")]
    public class AnnonceController : ControllerBase
    {
        private readonly SportTogetherContext _context;
        private readonly IConfiguration _configuration;

        public AnnonceController(IConfiguration configuration, SportTogetherContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: ApiSportTogether/Annonce
        [HttpGet]
        public ActionResult<IEnumerable<Annonce>> GetAnnonces()
        {


            return _context.Annonces
                           .Include(a => a.AuteurNavigation)
                           .Include(a => a.Sport)
                           .ToArray();
        }

        // GET: ApiSportTogether/Annonce/5
        [HttpGet("{id}")]
        public ActionResult<Annonce> GetAnnonceById(int id)
        {
            var annonce = _context.Annonces
                                  .Include(a => a.AuteurNavigation)
                                  .Include(a => a.Sport)

                                  .FirstOrDefault(a => a.AnnoncesId == id);

            return annonce == null ? NotFound() : annonce;
        }

        // POST: ApiSportTogether/Annonce/CreateAnnonce
        [HttpPost("CreateAnnonce")]
        public ActionResult<Annonce> PostAnnonce([FromBody] Annonce annonce)
        {
            Groupe? groupe = null;
            if (annonce == null)
            {
                return BadRequest("L'annonce ne peut pas être nulle.");
            }

            if (string.IsNullOrEmpty(annonce.Titre))
            {
                return BadRequest("Le titre est requis.");
            }

            if (string.IsNullOrEmpty(annonce.Description))
            {
                return BadRequest("La description est requise.");
            }

            if (annonce.DateHeureAnnonce < DateTime.Now)
            {
                return BadRequest("La date de l'annonce doit être égale ou supérieure à la date actuelle.");
            }
            // Créer une instance de VerificateurDeTexte
            VerificateurDeTexte verificateurDeTexte = new VerificateurDeTexte();
            // Vérifier le texte pour des mots racistes ou sexistes
            var (isClean, motsTrouves) = verificateurDeTexte.VerifierTexte(annonce.Titre);

            if (!isClean)
            {
                return BadRequest(new
                {
                    Message = "Le texte contient des mots interdits.",
                    MotsTrouves = motsTrouves
                });
            }

            // Vérifier le texte pour des mots racistes ou sexistes
            var (isCleanDesc, motsTrouvesDesc) = verificateurDeTexte.VerifierTexte(annonce.Description!);

            if (!isCleanDesc)
            {
                return BadRequest(new
                {
                    Message = "Le texte contient des mots interdits.",
                    MotsTrouves = motsTrouvesDesc
                });
            }

            _context.Annonces.Add(annonce);
            _context.SaveChanges();

            Annonce? annoncePourVerif = _context.Annonces.Find(annonce.AnnoncesId);

            if (annoncePourVerif != null)
            {
                if (!annoncePourVerif.Groupes.Any())
                {
                    groupe = new()
                    {
                        Annonce = annoncePourVerif,
                        AnnonceId = annoncePourVerif.AnnoncesId,
                        DateCreation = DateTime.Now,
                        ChefDuGroupe = annoncePourVerif.Auteur,
                        ChefDuGroupeNavigation = annoncePourVerif.AuteurNavigation!,
                        Nom = annoncePourVerif.Titre,

                    };
                    _context.Groupes.Add(groupe);

                }

            }
            _context.SaveChanges();


            if (groupe != null)
            {
                MembreGroupe mg = new()
                {
                    GroupeId = groupe.GroupesId,
                    Role = "Admin",
                    UtilisateurId = groupe.ChefDuGroupe
                };
                _context.MembreGroupes.Add(mg);
            }
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAnnonceById), new { id = annonce.AnnoncesId }, annonce);
        }

        // PUT: ApiSportTogether/Annonce/5
        [HttpPut("{id}")]
        public IActionResult PutAnnonce(int id, Annonce annonce)
        {
            if (id != annonce.AnnoncesId)
            {
                return BadRequest();
            }

            _context.Entry(annonce).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Annonces.Any(a => a.AnnoncesId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: ApiSportTogether/Annonce/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnnonce(int id)
        {
            var annonce = _context.Annonces.Find(id);
            Groupe? groupe = _context.Groupes.Where(g => g.AnnonceId == id).Include(g => g.MembreGroupes).FirstOrDefault();

            if (groupe == null) { return NotFound(); }

            if (annonce == null)
            {
                return NotFound();
            }
            List<Message>? listMessage = _context.Messages
                                  .Where(m => m.GroupeId == groupe.GroupesId)
                                  .Include(m => m.VuMessages)
                                  .OrderBy(m => m.Timestamp)
                                  .ToList();


            _context.MembreGroupes.RemoveRange(groupe.MembreGroupes);
            await _context.SaveChangesAsync();

            foreach (Message message in listMessage)
            {
                _context.VuMessages.RemoveRange(message.VuMessages);
                await _context.SaveChangesAsync();

                _context.Messages.Remove(message);
                await _context.SaveChangesAsync();
            }
            List<Participation> listParticipation = new();
            listParticipation = _context.Participations.Where(p => p.AnnonceId == annonce.AnnoncesId).ToList();
            if (listParticipation.Any())
            {
                _context.Participations.RemoveRange(listParticipation);
                await _context.SaveChangesAsync();

            }
            _context.Annonces.Remove(annonce);
            await _context.SaveChangesAsync();
            _context.Groupes.Remove(groupe);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // GET: ApiSportTogether/Annonce/vue/genre/ville
        [HttpGet("/vue/{genre}/{ville}/{indexPage}")]
        public ActionResult<IEnumerable<AnnonceVue>> GetAnnonceVue(string genre, string ville, int indexPage)
        {
            DateTime dateDuJour = DateTime.Now;
            List<Annonce> listAnnonce = new();
            List<AnnonceVue> listAnnonceVue = new();
            listAnnonce = _context.Annonces.Where(a => a.Ville == ville && a.DateHeureAnnonce > dateDuJour).OrderBy(a => a.DateHeureAnnonce).Include(a => a.Sport).Include(a => a.AuteurNavigation).ToList();
            if (listAnnonce.Count > 0)
            {
                listAnnonce = listAnnonce.Where(la => la.GenreAttendu == genre || la.GenreAttendu == "Mixte").ToList();
                foreach (Annonce annonce in listAnnonce)
                {
                        AnnonceVue annonceVueFirst = new()
                        {
                            AnnoncesId = annonce.AnnoncesId,
                            DateHeureAnnonce = annonce.DateHeureAnnonce,
                            Auteur = annonce.AuteurNavigation?.Pseudo ?? string.Empty,
                            AuteurId = annonce.Auteur,
                            Description = annonce.Description,
                            GenreAttendu = annonce.GenreAttendu,
                            Lieu = annonce.Lieu,
                            SportId = annonce.SportId,
                            SportName = annonce.Sport?.Nom ?? string.Empty,
                            NombreParticipants = annonce.NombreParticipants,
                            Titre = annonce.Titre,
                            Ville = annonce.Ville,
                            TotalAnnonce = listAnnonce.Count,
                            Niveau = annonce.Niveau
                        };
                        listAnnonceVue.Add(annonceVueFirst);
                  
                  


                   
                }
                // Trier les annonces par NiveauUtilisateur et NiveauAnnonce
                if (listAnnonceVue.Count > 0)
                {
                    listAnnonceVue = listAnnonceVue
                        .OrderBy(a => _context.Utilisateurs.FirstOrDefault(u => u.UtilisateursId == a.AuteurId)?.NiveauSport) // Trier par niveau utilisateur
                        .ThenBy(a => _context.Annonces.FirstOrDefault(an => an.AnnoncesId == a.AnnoncesId)?.Niveau) // Puis par niveau de l'annonce
                        .ToList();
                }
                return listAnnonceVue.Skip((indexPage - 1) * 12).Take(12).ToArray();
            }
            else
            {
                return NotFound();
            }
        }

        // GET: ApiSportTogether/Annonce/vue/sports
        [HttpGet("/vue/sports/{sports}/{ville}/{genreUtilisateur}/{indexPage}")]
        public ActionResult<IEnumerable<AnnonceVue>> GetAnnonceVueBySports(string sports, string ville, string genreUtilisateur, int indexPage)
        {
            DateTime dateDuJour = DateTime.Now;
            List<string> sportsList = sports.Split(',').ToList();
            List<Annonce> listAnnonce = _context.Annonces
                                                 .Where(a => sportsList.Contains(a.Sport.Nom) && a.DateHeureAnnonce > dateDuJour)
                                                 .Where(a => a.Ville == ville)
                                                 .OrderBy(a => a.DateHeureAnnonce)
                                                 .Include(a => a.Sport)
                                                 .Include(a => a.AuteurNavigation)
                                                 .ToList();

            if (listAnnonce.Count > 0)
            {
                listAnnonce = listAnnonce.Where(la => la.GenreAttendu == genreUtilisateur || la.GenreAttendu == "Mixte").ToList();
                List<AnnonceVue> listAnnonceVue = listAnnonce.Select(annonce => new AnnonceVue
                {
                    AnnoncesId = annonce.AnnoncesId,
                    DateHeureAnnonce = annonce.DateHeureAnnonce,
                    Auteur = annonce.AuteurNavigation?.Pseudo ?? string.Empty,
                    AuteurId = annonce.Auteur,
                    Description = annonce.Description,
                    GenreAttendu = annonce.GenreAttendu,
                    Lieu = annonce.Lieu,
                    SportId = annonce.SportId,
                    SportName = annonce.Sport?.Nom ?? string.Empty,
                    NombreParticipants = annonce.NombreParticipants,
                    Titre = annonce.Titre,
                    Ville = annonce.Ville,
                    TotalAnnonce = listAnnonce.Count,
                    Niveau = annonce.Niveau
                }).ToList();
                
                if (listAnnonceVue.Count > 0)
                {
                  
                    listAnnonceVue = listAnnonceVue
                        .OrderBy(a => _context.Utilisateurs.FirstOrDefault(u => u.UtilisateursId == a.AuteurId)?.NiveauSport) // Trier par niveau utilisateur
                        .ThenBy(a => _context.Annonces.FirstOrDefault(an => an.AnnoncesId == a.AnnoncesId)?.Niveau) // Puis par niveau de l'annonce
                        .ToList();
                }
                return listAnnonceVue.Skip((indexPage - 1) * 12).Take(12).ToArray();
            }
            else
            {
                return NotFound();
            }
        } // GET: ApiSportTogether/Annonce/vue/genre
        [HttpGet("/vue/genre/{genreUtilisateur}/{ville}/{indexPage}")]
        public ActionResult<IEnumerable<AnnonceVue>> GetAnnonceVueByGenre(string ville, string genreUtilisateur, int indexPage)
        {
            DateTime dateDuJour = DateTime.Now;
            List<Annonce> listAnnonce = _context.Annonces
                                                 .Where(a => a.Ville == ville && a.DateHeureAnnonce > dateDuJour)
                                                 .OrderBy(a => a.DateHeureAnnonce)
                                                 .Include(a => a.Sport)
                                                 .Include(a => a.AuteurNavigation)
                                                 .ToList();

            if (listAnnonce.Count > 0)
            {
                listAnnonce = listAnnonce.Where(a => a.GenreAttendu == genreUtilisateur).ToList();
                List<AnnonceVue> listAnnonceVue = listAnnonce.Select(annonce => new AnnonceVue
                {
                    AnnoncesId = annonce.AnnoncesId,
                    DateHeureAnnonce = annonce.DateHeureAnnonce,
                    Auteur = annonce.AuteurNavigation?.Pseudo ?? string.Empty,
                    AuteurId = annonce.Auteur,
                    Description = annonce.Description,
                    GenreAttendu = annonce.GenreAttendu,
                    Lieu = annonce.Lieu,
                    SportId = annonce.SportId,
                    SportName = annonce.Sport?.Nom ?? string.Empty,
                    NombreParticipants = annonce.NombreParticipants,
                    Titre = annonce.Titre,
                    Ville = annonce.Ville,
                    TotalAnnonce = listAnnonce.Count,
                    Niveau = annonce.Niveau
                }).ToList();
                if (listAnnonceVue.Count > 0)
                {
                    
                    listAnnonceVue = listAnnonceVue
                        .OrderBy(a => _context.Utilisateurs.FirstOrDefault(u => u.UtilisateursId == a.AuteurId)?.NiveauSport) // Trier par niveau utilisateur
                        .ThenBy(a => _context.Annonces.FirstOrDefault(an => an.AnnoncesId == a.AnnoncesId)?.Niveau) // Puis par niveau de l'annonce
                        .ToList();
                }
                return listAnnonceVue.Skip((indexPage - 1) * 12).Take(12).ToArray();
            }
            else
            {
                return NotFound();
            }
        }
        // GET: ApiSportTogether/Annonce/vue/villes
        [HttpGet("/vue/villes/{villes}/{genreUtilisateur}/{indexPage}")]
        public ActionResult<IEnumerable<AnnonceVue>> GetAnnonceVueByVilles(string villes, string genreUtilisateur, int indexPage)
        {
            DateTime dateDuJour = DateTime.Now;
            List<string> villesList = villes.Split(',').ToList();
            List<Annonce> listAnnonce = _context.Annonces
                                                 .Where(a => villesList.Contains(a.Ville) && a.DateHeureAnnonce > dateDuJour)
                                                 .OrderBy(a => a.DateHeureAnnonce)
                                                 .Include(a => a.Sport)
                                                 .Include(a => a.AuteurNavigation)
                                                 .ToList();

            if (listAnnonce.Count > 0)
            {
                listAnnonce = listAnnonce.Where(la => la.GenreAttendu == genreUtilisateur || la.GenreAttendu == "Mixte").ToList();
                List<AnnonceVue> listAnnonceVue = listAnnonce.Select(annonce => new AnnonceVue
                {
                    AnnoncesId = annonce.AnnoncesId,
                    DateHeureAnnonce = annonce.DateHeureAnnonce,
                    Auteur = annonce.AuteurNavigation?.Pseudo ?? string.Empty,
                    AuteurId = annonce.Auteur,
                    Description = annonce.Description,
                    GenreAttendu = annonce.GenreAttendu,
                    Lieu = annonce.Lieu,
                    SportId = annonce.SportId,
                    SportName = annonce.Sport?.Nom ?? string.Empty,
                    NombreParticipants = annonce.NombreParticipants,
                    Titre = annonce.Titre,
                    Ville = annonce.Ville,
                    TotalAnnonce = listAnnonce.Count,
                    Niveau = annonce.Niveau
                }).ToList();
                if (listAnnonceVue.Count > 0)
                {
                   
                    listAnnonceVue = listAnnonceVue
                        .OrderBy(a => _context.Utilisateurs.FirstOrDefault(u => u.UtilisateursId == a.AuteurId)?.NiveauSport) // Trier par niveau utilisateur
                        .ThenBy(a => _context.Annonces.FirstOrDefault(an => an.AnnoncesId == a.AnnoncesId)?.Niveau) // Puis par niveau de l'annonce
                        .ToList();
                }
                return listAnnonceVue.Skip((indexPage - 1) * 12).Take(12).ToArray();
            }
            else
            {
                return NotFound();
            }
        }

        // GET: ApiSportTogether/Annonce/vue/titre
        [HttpGet("/vue/titre/{motCle}/{genreUtilisateur}/{ville}/{indexPage}")]
        public ActionResult<IEnumerable<AnnonceVue>> GetAnnonceVueByTitre(string motCle, string ville, string genreUtilisateur, int indexPage)
        {
            DateTime dateDuJour = DateTime.Now;
            List<Annonce> listAnnonce = _context.Annonces
                                                 .Where(a => a.Titre.Contains(motCle) && a.DateHeureAnnonce > dateDuJour)
                                                 .Where(a => a.Ville == ville)
                                                 .OrderBy(a => a.DateHeureAnnonce)
                                                 .Include(a => a.Sport)
                                                 .Include(a => a.AuteurNavigation)
                                                 .ToList();

            if (listAnnonce.Count > 0)
            {
                listAnnonce = listAnnonce.Where(la => la.GenreAttendu == genreUtilisateur || la.GenreAttendu == "Mixte").ToList();
                List<AnnonceVue> listAnnonceVue = listAnnonce.Select(annonce => new AnnonceVue
                {
                    AnnoncesId = annonce.AnnoncesId,
                    DateHeureAnnonce = annonce.DateHeureAnnonce,
                    Auteur = annonce.AuteurNavigation?.Pseudo ?? string.Empty,
                    AuteurId = annonce.Auteur,
                    Description = annonce.Description,
                    GenreAttendu = annonce.GenreAttendu,
                    Lieu = annonce.Lieu,
                    SportId = annonce.SportId,
                    SportName = annonce.Sport?.Nom ?? string.Empty,
                    NombreParticipants = annonce.NombreParticipants,
                    Titre = annonce.Titre,
                    Ville = annonce.Ville,
                    TotalAnnonce = listAnnonce.Count,
                    Niveau = annonce.Niveau
                }).ToList();
                if (listAnnonceVue.Count > 0)
                {
                    listAnnonceVue.First().TotalAnnonce = listAnnonceVue.Count;
                    listAnnonceVue = listAnnonceVue
                        .OrderBy(a => _context.Utilisateurs.FirstOrDefault(u => u.UtilisateursId == a.AuteurId)?.NiveauSport) // Trier par niveau utilisateur
                        .ThenBy(a => _context.Annonces.FirstOrDefault(an => an.AnnoncesId == a.AnnoncesId)?.Niveau) // Puis par niveau de l'annonce
                        .ToList();
                }
                return listAnnonceVue.Skip((indexPage - 1) * 12).Take(12).ToArray();
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("/annonces/auteur/{utilisateurId}/{indexPage}")]
        public ActionResult<IEnumerable<AnnonceVue>> GetAnnoncesByAuteur(int utilisateurId, int indexPage)
        {
            DateTime dateDuJour = DateTime.Now;
            List<Annonce> listAnnonce = _context.Annonces
                                                 .Where(a => a.Auteur == utilisateurId && a.DateHeureAnnonce > dateDuJour)
                                                 .OrderBy(a => a.DateHeureAnnonce)
                                                 .Include(a => a.Sport)
                                                 .Include(a => a.AuteurNavigation)
                                                 .ToList();

            if (listAnnonce.Count > 0)
            {
                List<AnnonceVue> listAnnonceVue = listAnnonce.Select(annonce => new AnnonceVue
                {
                    AnnoncesId = annonce.AnnoncesId,
                    DateHeureAnnonce = annonce.DateHeureAnnonce,
                    Auteur = annonce.AuteurNavigation?.Pseudo ?? string.Empty,
                    AuteurId = annonce.Auteur,
                    Description = annonce.Description,
                    GenreAttendu = annonce.GenreAttendu,
                    Lieu = annonce.Lieu,
                    SportId = annonce.SportId,
                    SportName = annonce.Sport?.Nom ?? string.Empty,
                    NombreParticipants = annonce.NombreParticipants,
                    Titre = annonce.Titre,
                    Ville = annonce.Ville,
                    TotalAnnonce = listAnnonce.Count,
                    Niveau = annonce.Niveau
                }).ToList();
                if(listAnnonceVue.Count > 0)
                {
                    listAnnonceVue.First().TotalAnnonce = listAnnonceVue.Count;
                }
                return listAnnonceVue.Skip((indexPage - 1) * 12).Take(12).ToArray();
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("/annonces/participant/{utilisateurId}/{indexPage}")]
        public ActionResult<IEnumerable<AnnonceVue>> GetAnnoncesByParticipant(int utilisateurId, int indexPage)
        {
            DateTime dateDuJour = DateTime.Now;

            // Obtenez toutes les participations de l'utilisateur
            List<int?> annonceIds = _context.Participations
                .Where(p => p.UtilisateurId == utilisateurId)
                .Select(p => p.AnnonceId)
                .ToList();
            if (annonceIds != null)
            {
                List<Annonce> listAnnonce = _context.Annonces
                .Where(a => annonceIds.Contains(a.AnnoncesId) && a.DateHeureAnnonce > dateDuJour)
                .OrderBy(a => a.DateHeureAnnonce)
                .Include(a => a.Sport)
                .Include(a => a.AuteurNavigation)
                .ToList();

                if (listAnnonce.Count > 0)
                {
                    List<AnnonceVue> listAnnonceVue = listAnnonce.Select(annonce => new AnnonceVue
                    {
                        AnnoncesId = annonce.AnnoncesId,
                        DateHeureAnnonce = annonce.DateHeureAnnonce,
                        Auteur = annonce.AuteurNavigation?.Pseudo ?? string.Empty,
                        AuteurId = annonce.Auteur,
                        Description = annonce.Description,
                        GenreAttendu = annonce.GenreAttendu,
                        Lieu = annonce.Lieu,
                        SportId = annonce.SportId,
                        SportName = annonce.Sport?.Nom ?? string.Empty,
                        NombreParticipants = annonce.NombreParticipants,
                        Titre = annonce.Titre,
                        Ville = annonce.Ville,
                        TotalAnnonce = listAnnonce.Count,
                        Niveau = annonce.Niveau
                    }).ToList();
                    if (listAnnonceVue.Count > 0)
                    {
                        listAnnonceVue.First().TotalAnnonce = listAnnonceVue.Count;
                        listAnnonceVue = listAnnonceVue
                            .OrderBy(a => _context.Utilisateurs.FirstOrDefault(u => u.UtilisateursId == a.AuteurId)?.NiveauSport) // Trier par niveau utilisateur
                            .ThenBy(a => _context.Annonces.FirstOrDefault(an => an.AnnoncesId == a.AnnoncesId)?.Niveau) // Puis par niveau de l'annonce
                            .ToList();
                    }
                    return Ok(listAnnonceVue.Skip((indexPage - 1) * 12).Take(12).ToArray());   
                }
                else
                {
                    return NotFound();
                }


            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("/annonces/historique/{utilisateurId}/{indexPage}")]
        public ActionResult<IEnumerable<AnnonceVue>> GetAnnoncesByHistorique(int utilisateurId, int indexPage)
        {
            DateTime dateDuJour = DateTime.Now;

            // Obtenir toutes les participations de l'utilisateur
            List<int?> annonceIdsParticipant = _context.Participations
                .Where(p => p.UtilisateurId == utilisateurId)
                .Select(p => p.AnnonceId)
                .ToList();

            // Obtenir toutes les annonces où l'utilisateur est l'auteur
            List<Annonce> listAnnonceAuteur = _context.Annonces
                .Where(a => a.Auteur == utilisateurId && a.DateHeureAnnonce < dateDuJour)
                .Include(a => a.Sport)
                .Include(a => a.AuteurNavigation)
                .ToList();

            // Obtenir toutes les annonces où l'utilisateur est participant
            List<Annonce> listAnnonceParticipant = _context.Annonces
                .Where(a => annonceIdsParticipant.Contains(a.AnnoncesId) && a.DateHeureAnnonce < dateDuJour)
                .Include(a => a.Sport)
                .Include(a => a.AuteurNavigation)
                .ToList();

            // Combiner les annonces d'auteur et de participant
            List<Annonce> combinedAnnonces = listAnnonceAuteur
                .Union(listAnnonceParticipant) // Union pour combiner les deux listes sans doublons
                .OrderBy(a => a.DateHeureAnnonce) // Trier par la date la plus proche de maintenant
                .ToList();

            if (combinedAnnonces.Count > 0)
            {
                List<AnnonceVue> listAnnonceVue = combinedAnnonces.Select(annonce => new AnnonceVue
                {
                    AnnoncesId = annonce.AnnoncesId,
                    DateHeureAnnonce = annonce.DateHeureAnnonce,
                    Auteur = annonce.AuteurNavigation?.Pseudo ?? string.Empty,
                    AuteurId = annonce.Auteur,
                    Description = annonce.Description,
                    GenreAttendu = annonce.GenreAttendu,
                    Lieu = annonce.Lieu,
                    SportId = annonce.SportId,
                    SportName = annonce.Sport?.Nom ?? string.Empty,
                    NombreParticipants = annonce.NombreParticipants,
                    Titre = annonce.Titre,
                    Ville = annonce.Ville,
                    TotalAnnonce = combinedAnnonces.Count,
                    Niveau = annonce.Niveau
                }).ToList();
                if (listAnnonceVue.Count > 0)
                {
                 
                    listAnnonceVue = listAnnonceVue
                        .OrderBy(a => _context.Utilisateurs.FirstOrDefault(u => u.UtilisateursId == a.AuteurId)?.NiveauSport) // Trier par niveau utilisateur
                        .ThenBy(a => _context.Annonces.FirstOrDefault(an => an.AnnoncesId == a.AnnoncesId)?.Niveau) // Puis par niveau de l'annonce
                        .ToList();
                }
                return Ok(listAnnonceVue.Skip((indexPage - 1) * 12).Take(12).ToArray());
            }
            else
            {
                return NotFound();
            }
        }



    }
}

