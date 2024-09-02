using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.ObjectContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiSportTogether.Controller
{
    [ApiController]
    [Route("ApiSportTogether/[controller]")]
    public class MembreGroupeController : ControllerBase
    {
        private readonly SportTogetherContext _context;
        private readonly IConfiguration _configuration;

        public MembreGroupeController(IConfiguration configuration, SportTogetherContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: ApiSportTogether/MembreGroupe
        [HttpGet]
        public ActionResult<IEnumerable<MembreGroupe>> GetMembreGroupes()
        {
            return _context.MembreGroupes
                           .Include(mg => mg.Groupe)
                           .Include(mg => mg.Utilisateur)
                           .ToArray();
        }

        // GET: ApiSportTogether/MembreGroupe/5
        [HttpGet("{id}")]
        public ActionResult<MembreGroupe> GetMembreGroupeById(int id)
        {
            var membreGroupe = _context.MembreGroupes
                                       .Include(mg => mg.Groupe)
                                       .Include(mg => mg.Utilisateur)
                                       .FirstOrDefault(mg => mg.IdMembreGroupe == id);

            return membreGroupe == null ? NotFound() : membreGroupe;
        }

        // POST: ApiSportTogether/MembreGroupe
        [HttpPost]
        public ActionResult<MembreGroupe> PostMembreGroupe([FromBody] MembreGroupe membreGroupe)
        {
            _context.MembreGroupes.Add(membreGroupe);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetMembreGroupeById), new { id = membreGroupe.IdMembreGroupe }, membreGroupe);
        }

        // PUT: ApiSportTogether/MembreGroupe/5
        [HttpPut("{id}")]
        public IActionResult PutMembreGroupe(int id, MembreGroupe membreGroupe)
        {
            if (id != membreGroupe.IdMembreGroupe)
            {
                return BadRequest();
            }

            _context.Entry(membreGroupe).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.MembreGroupes.Any(mg => mg.IdMembreGroupe == id))
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

        // DELETE: ApiSportTogether/MembreGroupe/5
        [HttpDelete("{id}")]
        public IActionResult DeleteMembreGroupe(int id)
        {
            var membreGroupe = _context.MembreGroupes.Find(id);
            if (membreGroupe == null)
            {
                return NotFound();
            }

            _context.MembreGroupes.Remove(membreGroupe);
            _context.SaveChanges();

            return NoContent();
        }

        // GET: ApiSportTogether/MembreGroupe/GetMembresByGroupe/5
        [HttpGet("GetMembresByGroupe/{groupeId}")]
        public ActionResult<IEnumerable<MembreGroupe>> GetMembresByGroupe(int groupeId)
        {
            var membres = _context.MembreGroupes
                                  .Include(mg => mg.Utilisateur)
                                  .Where(mg => mg.GroupeId == groupeId)
                                  .ToList();

            return !membres.Any() ? NoContent() : Ok(membres.ToArray());
        }

        // GET: ApiSportTogether/MembreGroupe/GetGroupesByUtilisateur/5
        [HttpGet("GetGroupesByUtilisateur/{utilisateurId}")]
        public ActionResult<IEnumerable<MembreGroupe>> GetGroupesByUtilisateur(int utilisateurId)
        {
            var groupes = _context.MembreGroupes
                                  .Include(mg => mg.Groupe)
                                  .Where(mg => mg.UtilisateurId == utilisateurId)
                                  .ToList();

            return !groupes.Any() ? NoContent() : Ok(groupes.ToArray());
        }
    }
}
