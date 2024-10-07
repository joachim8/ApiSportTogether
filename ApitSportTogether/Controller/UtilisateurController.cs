using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.ObjectContext;
using ApiSportTogether.model.ObjectVue;
using ApiSportTogether.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ApiSportTogether.Controller
{
    [ApiController]
    [Route("ApiSportTogether/[controller]")]
    public class UtilisateurController : ControllerBase
    {
        private readonly SportTogetherContext _context;
        private readonly IConfiguration _configuration;

        public UtilisateurController(IConfiguration configuration, SportTogetherContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: ApiSportTogether/Utilisateur
        [HttpGet]
        public ActionResult<List<Utilisateur>> GetUtilisateurs()
        {
            return _context.Utilisateurs
                           .Include(u => u.ProfileImages)
                           .ToList();
        }

        // GET: ApiSportTogether/Utilisateur/5
        [HttpGet("{id}")]
        public ActionResult<Utilisateur> GetUtilisateurById(int id)
        {
            var utilisateur = _context.Utilisateurs
                                       .Include(u => u.ProfileImages)
                                       .FirstOrDefault(u => u.UtilisateursId == id);

            return utilisateur == null ? NotFound() : utilisateur;
        }

        // POST: ApiSportTogether/Utilisateur/CreateUtilisateur
        [HttpPost("CreateUtilisateur")]
        public ActionResult<Utilisateur> PostUtilisateur([FromBody]Utilisateur utilisateur)
        {
            if (utilisateur != null)
            {

                

                utilisateur.MotDePasse = HashPassword(utilisateur.MotDePasse);
                _context.Utilisateurs.Add(utilisateur);
                _context.SaveChanges();
                if(utilisateur.UtilisateursId != 0)
                {
                    ProfileImage profileImage = new()
                    {
                        UtilisateursId = utilisateur.UtilisateursId,
                        Timestamp = DateTime.Now,
                        Type = "Profil",
                        Url = "http://localhost:5000/Images/avatar-photo-profil.jpg"
                    };
                    _context.ProfileImages.Add(profileImage);
                    _context.SaveChanges();
                }
                return CreatedAtAction(nameof(GetUtilisateurById), new { id = utilisateur.UtilisateursId }, utilisateur);
            }
            else
            { return NotFound(); }

        }

        // PUT: ApiSportTogether/Utilisateur/5
        [HttpPut("{id}")]
        public ActionResult PutUtilisateur(int id, Utilisateur utilisateur)
        {
            // Créer une instance de VerificateurDeTexte
            VerificateurDeTexte verificateurDeTexte = new VerificateurDeTexte();
            // Vérifier le texte pour des mots racistes ou sexistes
            var (isClean, motsTrouves) = verificateurDeTexte.VerifierTexte(utilisateur.Description!);

            if (!isClean)
            {
                return BadRequest(new
                {
                    Message = "Le texte contient des mots interdits.",
                    MotsTrouves = motsTrouves
                });
            }// Vérifier le texte pour des mots racistes ou sexistes
            var (isCleanDesc, motsTrouvesDesc) = verificateurDeTexte.VerifierTexte(utilisateur.DescriptionSport!);

            if (!isCleanDesc)
            {
                return BadRequest(new
                {
                    Message = "Le texte contient des mots interdits.",
                    MotsTrouves = motsTrouvesDesc
                });
            }// Vérifier le texte pour des mots racistes ou sexistes

            var (isCleanFun, motsTrouvesFun) = verificateurDeTexte.VerifierTexte(utilisateur.FunFact!);

            if (!isCleanFun)
            {
                return BadRequest(new
                {
                    Message = "Le texte contient des mots interdits.",
                    MotsTrouves = motsTrouvesFun
                });
            }
            if (id != utilisateur.UtilisateursId)
            {
                return BadRequest();
            }
            if(utilisateur.MotDePasse != HashPassword(utilisateur.MotDePasse))  utilisateur.MotDePasse = HashPassword(utilisateur.MotDePasse);
            _context.Entry(utilisateur).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Utilisateurs.Any(u => u.UtilisateursId == id))
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
        // PUT: ApiSportTogether/Utilisateur/ModifierMotDePasse/1
        [HttpPut("ModifierMotDePasse/{id}")]
        public ActionResult ModifierMotDePasse(int id, [FromBody] string password)
        {
            Utilisateur? utilisateur = _context.Utilisateurs.Find(id);
            if (utilisateur == null)
            {
                return NotFound("L'utilisateur n'a pas été trouvée.");
            }
            utilisateur.MotDePasse = HashPassword(password);
            _context.Entry(utilisateur).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Utilisateurs.Any(u => u.UtilisateursId == id))
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

        private string HashPassword(string password)
        {
            var hasher = new PasswordHasher<object>();
            return hasher.HashPassword(null, password);
        }

        // DELETE: ApiSportTogether/Utilisateur/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUtilisateur(int id)
        {
            var utilisateur = _context.Utilisateurs.Find(id);
            if (utilisateur == null)
            {
                return NotFound();
            }

            _context.Utilisateurs.Remove(utilisateur);
            _context.SaveChanges();

            return NoContent();
        }


        // GET: utilisateur/GetUtilisateurVueByIdParMois/5/mois/2/annee/1
        [HttpGet("GetUtilisateurVueByIdParMois/{utilisateur_id}/mois/{mois}/annee/{annee}")]
        public ActionResult<UtilisateurVue> GetUtilisateurVueByIdParMois(int utilisateur_id, int mois, int annee)
        {
            // Récupérer l'utilisateur depuis la base de données
            var utilisateur = _context.Utilisateurs.FirstOrDefault(u => u.UtilisateursId == utilisateur_id);

            if (utilisateur == null)
            {
                return NotFound();
            }
            decimal? pourcentageAugmentationAnnonce = CalculerPourcentageAugmentation(utilisateur_id, mois, annee);
            // Exemple : Récupérer les données supplémentaires (à ajuster selon ton modèle et tes calculs)
            int nombreAuteurAnnonce = _context.Annonces.Count(a => a.Auteur == utilisateur_id);
            int nombreAnnonceEffectuer = _context.Participations.Count(p => p.UtilisateurId == utilisateur_id);
            List<Annonce>? listAnnonce = _context.Annonces.Where(a => a.Auteur == utilisateur_id).ToList();
            List<decimal?> listNoteAnnonce = new();
            decimal? noteMoyenneDesAnnonces = null;
            var listAmi = GetListAmi(utilisateur_id);
            Array? listClassementAmi = GetClassementAmisActivitesMois(utilisateur_id,mois, annee);
            

            if (listAnnonce != null)
            {
                if (listAnnonce.Any())
                {
                    foreach (var annonce in listAnnonce)
                    {
                        if(annonce.NoteAnnonce != 0 && annonce.NoteAnnonce != null)
                        listNoteAnnonce.Add(annonce.NoteAnnonce);
                    }
                    decimal noteTotale = 0;
                    foreach (var a in listNoteAnnonce)
                    {
                        noteTotale = (decimal)(noteTotale + a)!;
                    }
                    if (noteTotale > 0)
                    {
                        noteMoyenneDesAnnonces = noteTotale / listAnnonce.Count();
                    }

                }
            }

                    var premiereParticipationOuAnnonce = _context.Participations
            .Where(p => p.UtilisateurId == utilisateur_id)
            .OrderBy(p => p.DateParticipation)
            .Select(p => p.DateParticipation)
            .FirstOrDefault();  // Récupérer la première date de participation

            if (premiereParticipationOuAnnonce == DateTime.MinValue)
            {
                // Si aucune participation trouvée, on prend la date courante pour éviter une division par zéro
                premiereParticipationOuAnnonce = DateTime.Now;
            }

            decimal moisDepasses = ((DateTime.Now - premiereParticipationOuAnnonce).Value.Days / 30.0m);
            if (moisDepasses <= 0)
            {
                moisDepasses = 1;  // Pour éviter une division par 0, on force au moins 1 mois
            }

            decimal? annonceEffectuerParMoisMoyenne = nombreAnnonceEffectuer / moisDepasses;




            // Exemple : Récupérer les sports favoris (adapter selon tes relations)
            List<string> topTroisSport = _context.SportFavoris
                                        .Where(s => s.UtilisateursId == utilisateur_id)
                                        .Take(3)
                                        .Include(sf => sf.Sports)
                                        .Select(s => s.Sports!.Nom)
                                        .ToList()!;
            string urlProfilImage = _context.ProfileImages.Where(pi => pi.UtilisateursId == utilisateur_id).FirstOrDefault()!.Url!;
            // Convertir en UtilisateurVue
            UtilisateurVue utilisateurVue = ConvertirEnUtilisateurVue(utilisateur, nombreAuteurAnnonce, nombreAnnonceEffectuer, noteMoyenneDesAnnonces ?? 0, annonceEffectuerParMoisMoyenne ?? 0, topTroisSport, urlProfilImage, listClassementAmi, pourcentageAugmentationAnnonce, utilisateur.DescriptionSport, utilisateur.FunFact, utilisateur.NiveauSport, utilisateur.Disponibilites, utilisateur.TypePartenaire);
            try
            {
                // Retourner l'objet
                return Ok(utilisateurVue);
            }
            catch(Exception ex)
            {
                return BadRequest(new Exception(ex.Message));
            }
            
        }
        private  UtilisateurVue ConvertirEnUtilisateurVue(Utilisateur utilisateur, int nombreAuteurAnnonce, int nombreAnnonceEffectuer, decimal noteMoyenneDesAnnonces, decimal annonceEffectuerParMoisMoyenne, List<string> topTroisSport, string url, Array? listClassementAmi, decimal? pourcentageAugmentationAnnonce,string descriptionSport ,string funFact, string niveauSport, string disponibilites, string typePartenaire)
        {
            return new UtilisateurVue
            {
                UtilisateursId = utilisateur.UtilisateursId,
                Nom = utilisateur.Nom,
                Prenom = utilisateur.Prenom,
                Pseudo = utilisateur.Pseudo,
                Genre = utilisateur.Genre,
                Age = utilisateur.Age,
                Ville = utilisateur.Ville,
                Description = utilisateur.Description,
                NombreAuteurAnnonce = nombreAuteurAnnonce,
                NombreAnnonceEffectuer = nombreAnnonceEffectuer,
                NoteMoyenneDesAnnonces = Math.Round(noteMoyenneDesAnnonces, 2),
                AnnonceEffectuerParMoisMoyenne = annonceEffectuerParMoisMoyenne,
                TopTroisSport = topTroisSport.ToArray(),
                urlProfilImage = url,
                ClassementAmis = listClassementAmi,
                PourcentageAugmentationAnnonceAuteur = pourcentageAugmentationAnnonce,
                DescriptionSport = descriptionSport,
                FunFact = funFact,
                NiveauSport= niveauSport,
                Disponibilites = disponibilites, 
                TypePartenaire = typePartenaire,
                

              

            };
        }
        private decimal? CalculerPourcentageAugmentation(int utilisateurId, int month, int annee)
        {
            // Obtenir toutes les participations de l'utilisateur
            List<int?> annonceIdsParticipant = _context.Participations
                .Where(p => p.UtilisateurId == utilisateurId)
                .Select(p => p.AnnonceId)
                .ToList();
            // Calcul de l'augmentation des annonces entre le mois précédent et ce mois
            int annoncesMoisActuel = 0;
            int annoncesMoisPrecedent = 0;

            // Obtenir toutes les annonces où l'utilisateur est l'auteur
            List<Annonce> listAnnonceAuteur = _context.Annonces
                .Where(a => a.Auteur == utilisateurId && a.DateHeureAnnonce.Month == month)
                .Include(a => a.Sport)
                .Include(a => a.AuteurNavigation)
                .ToList();

            // Obtenir toutes les annonces où l'utilisateur est participant
            List<Annonce> listAnnonceParticipant = _context.Annonces
                .Where(a => annonceIdsParticipant.Contains(a.AnnoncesId) && a.DateHeureAnnonce.Month == month)
                .Include(a => a.Sport)
                .Include(a => a.AuteurNavigation)
                .ToList();

            // Combiner les annonces d'auteur et de participant
            List<Annonce> combinedAnnoncesDuMois = listAnnonceAuteur
                .Union(listAnnonceParticipant) // Union pour combiner les deux listes sans doublons
                .OrderBy(a => a.DateHeureAnnonce) // Trier par la date la plus proche de maintenant
                .ToList();

            annoncesMoisActuel = combinedAnnoncesDuMois.Count();

            // Obtenir toutes les annonces où l'utilisateur est l'auteur
            List<Annonce> listAnnonceAuteurMoisPrecedent = _context.Annonces
                .Where(a => a.Auteur == utilisateurId && a.DateHeureAnnonce.Month == (month -1))
                .Include(a => a.Sport)
                .Include(a => a.AuteurNavigation)
                .ToList();

            // Obtenir toutes les annonces où l'utilisateur est participant
            List<Annonce> listAnnonceParticipantMoisPrecedent = _context.Annonces
                .Where(a => annonceIdsParticipant.Contains(a.AnnoncesId) && a.DateHeureAnnonce.Month == (month -1))
                .Include(a => a.Sport)
                .Include(a => a.AuteurNavigation)
                .ToList();

            // Combiner les annonces d'auteur et de participant
            List<Annonce> combinedAnnoncesMoisPrecedent = listAnnonceAuteurMoisPrecedent
                .Union(listAnnonceParticipantMoisPrecedent) // Union pour combiner les deux listes sans doublons
                .OrderBy(a => a.DateHeureAnnonce) // Trier par la date la plus proche de maintenant
                .ToList();


            annoncesMoisPrecedent = combinedAnnoncesMoisPrecedent.Count();

            if (annoncesMoisPrecedent > 0 )  return ((annoncesMoisActuel - annoncesMoisPrecedent) / (decimal)annoncesMoisPrecedent) * 100;
            return null;
           
        }
        private IEnumerable<Utilisateur>? GetListAmi(int utilisateur_id)
        {
            if (utilisateur_id == 0) return null;

            // Récupérer la liste d'amis depuis la table Amis
            List<Ami> listAmi = _context.Amis
                                        .Where(a => a.UtilisateurId1 == utilisateur_id || a.UtilisateurId2 == utilisateur_id)
                                        .Include(a => a.UtilisateurId1Navigation).ThenInclude(u => u.ProfileImages)
                                        .Include(a => a.UtilisateurId2Navigation).ThenInclude(u => u.ProfileImages)
                                        .ToList();

            if (listAmi == null || listAmi.Count == 0) return null;

            // Construire la liste des amis (sélectionner l'ami en fonction de l'utilisateur)
            List<Utilisateur> listUtilisateur = listAmi.Select(a =>
                a.UtilisateurId1 == utilisateur_id ? a.UtilisateurId2Navigation : a.UtilisateurId1Navigation
            ).ToList()!;

            if (!listUtilisateur.Any()) return null;
            return listUtilisateur.ToArray();
        }
        public Array? GetClassementAmisActivitesMois(int utilisateurId, int mois, int annee)
        {
            // Récupérer la liste des amis
            var amis = GetListAmi(utilisateurId)?.ToList() ?? new List<Utilisateur>();

            // Ajouter l'utilisateur en cours dans la liste des amis
            var utilisateur = _context.Utilisateurs.FirstOrDefault(u => u.UtilisateursId == utilisateurId);
            if (utilisateur != null)
            {
                amis.Add(utilisateur);
            }

            if (!amis.Any())
            {
                return null;
            }

            // Classement basé sur le nombre d'activités durant le mois sélectionné
            var classementAmis = amis.Select(ami => new
            {
                Ami = ami,
                NombreActivites = _context.Participations
                    .Where(p => p.UtilisateurId == ami.UtilisateursId &&
                                p.DateParticipation.Value.Month == mois &&
                                p.DateParticipation.Value.Year == annee)
                    .Count() +
                    _context.Annonces
                    .Where(a => a.Auteur == ami.UtilisateursId &&
                                a.DateHeureAnnonce.Month == mois &&
                                a.DateHeureAnnonce.Year == annee)
                    .Count()
            })
            .OrderByDescending(x => x.NombreActivites)
            .Select((x, index) => new ClassementAmi
            {
                Pseudo = x.Ami.Pseudo,
                UrlProfilImage = x.Ami.ProfileImages.FirstOrDefault()?.Url,
                Classement = index + 1,
                NombreActivites = x.NombreActivites
            })
            .Take(10)
            .ToList();

            if (!classementAmis.Any())
            {
                return null;
            }

            return classementAmis.OrderByDescending(ca => ca.NombreActivites).ToArray();
        }


        // GET: utilisateur/GetProfilUtilisateurByIdParMois/5
        [HttpGet("GetProfilUtilisateurVueByIdParMois/{utilisateur_id}/{utilisateur_en_cours_id}")]
        public ActionResult<ProfilUtilisateurVu> GetProfilUtilisateurByIdParMois(int utilisateur_id, int utilisateur_en_cours_id)
        {
            // Récupérer l'utilisateur depuis la base de données
            var utilisateur = _context.Utilisateurs.FirstOrDefault(u => u.UtilisateursId == utilisateur_id);

            if (utilisateur == null)
            {
                return NotFound();
            }
            decimal? pourcentageAugmentationAnnonce = null;
            // Exemple : Récupérer les données supplémentaires (à ajuster selon ton modèle et tes calculs)
            int nombreAuteurAnnonce = _context.Annonces.Count(a => a.Auteur == utilisateur_id);
            int nombreAnnonceEffectuer = _context.Participations.Count(p => p.UtilisateurId == utilisateur_id);
            List<Annonce>? listAnnonce = _context.Annonces.Where(a => a.Auteur == utilisateur_id).ToList();
            List<decimal?> listNoteAnnonce = new();
            decimal? noteMoyenneDesAnnonces = null;
            var listAmi = GetListAmi(utilisateur_id);
            Utilisateur? utilisateurEnLigne = _context.Utilisateurs.FirstOrDefault(u => u.UtilisateursId == utilisateur_en_cours_id);
            bool bIsCoequipier = false;


            if (listAnnonce != null)
            {
                if (listAnnonce.Any())
                {
                    foreach (var annonce in listAnnonce)
                    {
                        if (annonce.NoteAnnonce != null)
                            listNoteAnnonce.Add(annonce.NoteAnnonce);
                    }
                    decimal noteTotale = 0;
                    foreach (var a in listNoteAnnonce)
                    {
                        noteTotale = (decimal)(noteTotale + a)!;
                    }
                    if (noteTotale > 0)
                    {
                        noteMoyenneDesAnnonces = noteTotale / listAnnonce.Count();
                    }

                }
            }

            var premiereParticipationOuAnnonce = _context.Participations
    .Where(p => p.UtilisateurId == utilisateur_id)
    .OrderBy(p => p.DateParticipation)
    .Select(p => p.DateParticipation)
    .FirstOrDefault();  // Récupérer la première date de participation

            if (premiereParticipationOuAnnonce == DateTime.MinValue)
            {
                // Si aucune participation trouvée, on prend la date courante pour éviter une division par zéro
                premiereParticipationOuAnnonce = DateTime.Now;
            }

            decimal moisDepasses = ((DateTime.Now - premiereParticipationOuAnnonce).Value.Days / 30.0m);
            if (moisDepasses <= 0)
            {
                moisDepasses = 1;  // Pour éviter une division par 0, on force au moins 1 mois
            }

            decimal? annonceEffectuerParMoisMoyenne = nombreAnnonceEffectuer / moisDepasses;




            // Exemple : Récupérer les sports favoris (adapter selon tes relations)
            List<string> topTroisSport = _context.SportFavoris
                                        .Where(s => s.UtilisateursId == utilisateur_id)
                                        .Take(3)
                                        .Include(sf => sf.Sports)
                                        .Select(s => s.Sports!.Nom)
                                        .ToList()!;
            string urlProfilImage = _context.ProfileImages.Where(pi => pi.UtilisateursId == utilisateur_id).FirstOrDefault()!.Url!;

            if (utilisateur_id != utilisateur_en_cours_id)
            {
                if (listAmi.Any())
                {
                    if (listAmi!.Contains(utilisateurEnLigne))
                    {
                        bIsCoequipier = true;
                    }

                }
            }
               
            // Convertir en UtilisateurVue
            ProfilUtilisateurVu profilUtilisateurVu = new()
            {
                UtilisateursId = utilisateur_id,
                urlProfilImage = urlProfilImage,
                Age = utilisateur.Age,
                Description = utilisateur.Description,
                Genre = utilisateur.Genre,
                Nom = utilisateur.Nom,
                Pseudo = utilisateur.Pseudo,
                Prenom = utilisateur.Prenom,
                Ville = utilisateur.Ville,
                AnnonceEffectuerParMoisMoyenne = annonceEffectuerParMoisMoyenne,
                NombreAnnonceEffectuer = nombreAnnonceEffectuer,
                NombreAuteurAnnonce = nombreAuteurAnnonce,
                NoteMoyenneDesAnnonces = noteMoyenneDesAnnonces,
                TopTroisSport = topTroisSport.ToArray(),
                isCoequipier = bIsCoequipier,
                FunFact = utilisateur.FunFact,
                DescriptionSport = utilisateur.DescriptionSport,
                Disponibilites = utilisateur.Disponibilites,
                NiveauSport = utilisateur.NiveauSport,
                TypePartenaire = utilisateur.TypePartenaire
                
            };

            // Retourner l'objet
            return Ok(profilUtilisateurVu);
        }



    }
}
