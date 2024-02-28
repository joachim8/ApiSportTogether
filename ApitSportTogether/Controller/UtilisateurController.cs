using Microsoft.AspNetCore.Mvc;
using ApiSportTogether.model.ObjectContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiSportTogether.model.dbContext;

namespace ApiSportTogether.Controllers
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
        public ActionResult<IEnumerable<Utilisateur>> GetUtilisateurs()
        {
            return _context.Utilisateurs
                           .Include(u => u.Image)
                           .Include(u => u.AmiUtilisateurId1Navigations)
                           .Include(u => u.AmiUtilisateurId2Navigations)
                           .Include(u => u.Annonces)
                           .Include(u => u.Publications)
                           .ToList();
        }

        // GET: ApiSportTogether/Utilisateur/5
        [HttpGet("{id}")]
        public ActionResult<Utilisateur> GetUtilisateurById(int id)
        {
            var utilisateur = _context.Utilisateurs
                                       .Include(u => u.Image)
                                       .Include(u => u.AmiUtilisateurId1Navigations)
                                       .Include(u => u.AmiUtilisateurId2Navigations)
                                       .Include(u => u.Annonces)
                                       .Include(u => u.Publications)
                                       .FirstOrDefault(u => u.UserId == id);

            return utilisateur == null ? NotFound() : utilisateur;
        }

        // POST: ApiSportTogether/Utilisateur
        [HttpPost]
        public ActionResult<Utilisateur> PostUtilisateur(Utilisateur utilisateur)
        {
            _context.Utilisateurs.Add(utilisateur);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUtilisateurById), new { id = utilisateur.UserId }, utilisateur);
        }

        // PUT: ApiSportTogether/Utilisateur/5
        [HttpPut("{id}")]
        public IActionResult PutUtilisateur(int id, Utilisateur utilisateur)
        {
            if (id != utilisateur.UserId)
            {
                return BadRequest();
            }

            _context.Entry(utilisateur).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Utilisateurs.Any(u => u.UserId == id))
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
    }
}
