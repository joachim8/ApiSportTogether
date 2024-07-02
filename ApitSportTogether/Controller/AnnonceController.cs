using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.ObjectContext;
using ApiSportTogether.model.ObjectVue;
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
        public ActionResult<List<Annonce>> GetAnnonces()
        {


            return _context.Annonces
                           .Include(a => a.AuteurNavigation)
                           .Include(a => a.Sport)
                           .ToList();
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

            // Ajoutez d'autres vérifications nécessaires

            _context.Annonces.Add(annonce);
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
        public IActionResult DeleteAnnonce(int id)
        {
            var annonce = _context.Annonces.Find(id);
            if (annonce == null)
            {
                return NotFound();
            }

            _context.Annonces.Remove(annonce);
            _context.SaveChanges();

            return NoContent();
        }

    
        // GET: ApiSportTogether/Annonce/vue/genre/ville
        [HttpGet("/vue/{genre}/{ville}")]
        public ActionResult<IEnumerable<AnnonceVue>> GetAnnonceVue(string genre, string ville)
        {
            DateTime dateDuJour = DateTime.Now;
            List<Annonce> listAnnonce = new List<Annonce>();
            List<AnnonceVue> listAnnonceVue = new List<AnnonceVue>();
            listAnnonce =  _context.Annonces.Where(a => a.Ville == ville && a.DateHeureAnnonce > dateDuJour).OrderBy(a => a.DateHeureAnnonce).Include(a => a.Sport).Include(a => a.AuteurNavigation).ToList();
            if(listAnnonce.Count > 0)
            {
                listAnnonce = listAnnonce.Where(la => la.GenreAttendu == genre || la.GenreAttendu == "Mixte").ToList();
                foreach (Annonce annonce in listAnnonce)
                {
                    AnnonceVue annonceVue = new AnnonceVue
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
                        Ville = annonce.Ville
                    };


                    listAnnonceVue.Add(annonceVue);
                }
                return listAnnonceVue.ToArray();
            }
            else
            {
                return NotFound();
            }
        }

        // GET: ApiSportTogether/Annonce/vue/sports
        [HttpGet("/vue/sports/{sports}/{ville}/{genreUtilisateur}")]
        public ActionResult<IEnumerable<AnnonceVue>> GetAnnonceVueBySports(string sports,string ville, string  genreUtilisateur)
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
                    Ville = annonce.Ville
                }).ToList();

                return listAnnonceVue.ToArray();
            }
            else
            {
                return NotFound();
            }
        } // GET: ApiSportTogether/Annonce/vue/genre
        [HttpGet("/vue/genre/{genreUtilisateur}/{ville}")]
        public ActionResult<IEnumerable<AnnonceVue>> GetAnnonceVueByGenre(string ville, string  genreUtilisateur)
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
                    Ville = annonce.Ville
                }).ToList();

                return listAnnonceVue.ToArray();
            }
            else
            {
                return NotFound();
            }
        }
        // GET: ApiSportTogether/Annonce/vue/villes
        [HttpGet("/vue/villes/{villes}/{genreUtilisateur}")]
        public ActionResult<IEnumerable<AnnonceVue>> GetAnnonceVueByVilles(string villes, string genreUtilisateur)
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
                listAnnonce =  listAnnonce.Where(la => la.GenreAttendu == genreUtilisateur || la.GenreAttendu == "Mixte").ToList();
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
                    Ville = annonce.Ville
                }).ToList();

                return listAnnonceVue.ToArray();
            }
            else
            {
                return NotFound();
            }
        }
        
        // GET: ApiSportTogether/Annonce/vue/titre
        [HttpGet("/vue/titre/{motCle}/{genreUtilisateur}/{ville}")]
        public ActionResult<IEnumerable<AnnonceVue>> GetAnnonceVueByTitre(string motCle, string ville, string genreUtilisateur)
        {
            DateTime dateDuJour = DateTime.Now;
            List<Annonce> listAnnonce = _context.Annonces
                                                 .Where(a => a.Titre.Contains(motCle) && a.DateHeureAnnonce > dateDuJour )
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
                    Ville = annonce.Ville
                }).ToList();

                return listAnnonceVue.ToArray();
            }
            else
            {
                return NotFound();
            }
        }

    }
}

