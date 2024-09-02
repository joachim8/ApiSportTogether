using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.ObjectContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiSportTogether.Controller
{
    [ApiController]
    [Route("ApiSportTogether/[controller]")]
    public class GroupeController : ControllerBase
    {
        private readonly SportTogetherContext _context;
        private readonly IConfiguration _configuration;

        public GroupeController(IConfiguration configuration, SportTogetherContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: ApiSportTogether/Groupe
        [HttpGet]
        public ActionResult<IEnumerable<Groupe>> GetGroupes()
        {
            return _context.Groupes
                           .Include(g => g.Annonce)
                           .ToArray();
        }

        // GET: ApiSportTogether/Groupe/5
        [HttpGet("{id}")]
        public ActionResult<Groupe> GetGroupeById(int id)
        {
            var groupe = _context.Groupes
                                 .Include(g => g.Annonce)
                                 .FirstOrDefault(g => g.GroupesId == id);

            return groupe == null ? NotFound() : groupe;
        }

        // POST: ApiSportTogether/Groupe
        [HttpPost]
        public ActionResult<Groupe> PostGroupe([FromBody] Groupe groupe)
        {
            _context.Groupes.Add(groupe);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetGroupeById), new { id = groupe.GroupesId }, groupe);
        }

        // PUT: ApiSportTogether/Groupe/5
        [HttpPut("{id}")]
        public IActionResult PutGroupe(int id, Groupe groupe)
        {
            if (id != groupe.GroupesId)
            {
                return BadRequest();
            }

            _context.Entry(groupe).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Groupes.Any(g => g.GroupesId == id))
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

        // DELETE: ApiSportTogether/Groupe/5
        [HttpDelete("{id}")]
        public IActionResult DeleteGroupe(int id)
        {
            var groupe = _context.Groupes.Find(id);
            if (groupe == null)
            {
                return NotFound();
            }

            _context.Groupes.Remove(groupe);
            _context.SaveChanges();

            return NoContent();
        }
        // GET: ApiSportTogether/Groupe/GetGroupePourMessagerie/{UtilisateurID}
        [HttpGet("GetGroupePourMessagerie/{UtilisateurID}")]
        public ActionResult<IEnumerable<Groupe>> GetGroupePourMessagerie(int UtilisateurID)
        {
            List<Groupe>? listGroupe = [.. _context.Groupes.Where(g => g.ChefDuGroupe == UtilisateurID).ToList()];
            List<Participation> listParticipation = new();
            listParticipation = _context.Participations.Where(p => p.UtilisateurId == UtilisateurID).ToList();
            if(listParticipation.Any())
            {
                foreach(Participation participe in listParticipation)
                {
                    listGroupe.Add(_context.Groupes.Find(participe.GroupeId)!);
                }
            }
           

            if (!listGroupe.Any())
            {
                return NoContent();
            }
            else
            {
                foreach(Groupe groupe in listGroupe)
                {
                    groupe.Annonce = _context.Annonces.Find(groupe.AnnonceId);
                }
            }
            return listGroupe.OrderByDescending(lg => lg.DateCreation).ToArray();
        }
    }
}
