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
        public ActionResult<List<AnnonceVue>> GetAnnonceVue(string genre, string ville)
        {
            List<Annonce> listAnnonce = new List<Annonce>();
            List<AnnonceVue> listAnnonceVue = new List<AnnonceVue>();
            listAnnonce =  _context.Annonces.Where(a => a.GenreAttendu == genre || a.GenreAttendu == "Mixte" &&  a.Ville == ville).OrderBy(a => a.DateHeureAnnonce).Include(a => a.Sport).Include(a=> a.AnnonceImages).Include(a => a.AuteurNavigation).ToList();
            if(listAnnonce.Count > 0)
            {
                foreach(Annonce annonce in listAnnonce)
                {
                    AnnonceVue annonceVue = new()
                    {
                        AnnoncesId = annonce.AnnoncesId,
                        DateHeureAnnonce = annonce.DateHeureAnnonce,
                        Auteur =  annonce.AuteurNavigation.Pseudo!,
                        AuteurId = annonce.Auteur,
                        Description = annonce.Description,
                        GenreAttendu = annonce.GenreAttendu,
                       Lieu = annonce.Lieu,
                       SportId = annonce.SportId,
                       SportName = annonce.Sport!.Nom,
                       NombreParticipants = annonce.NombreParticipants,
                       ListAnnonceImage = (List<AnnonceImage>)annonce.AnnonceImages! ?? new List<AnnonceImage>(),
                       Titre = annonce.Titre,
                       Ville = annonce.Ville,
                    };

              
                    listAnnonceVue.Add(annonceVue);
                }
                return listAnnonceVue;
            }
            else
            {
                return NotFound();
            }
        }
    }
}

