using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.ObjectContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiSportTogether.Controller
{
    [ApiController]
    [Route("ApiSportTogether/[controller]")]
    public class EncouragementPublicationController : ControllerBase
    {
        private readonly SportTogetherContext _context;

        public EncouragementPublicationController(SportTogetherContext context)
        {
            _context = context;
        }

        // GET: ApiSportTogether/EncouragementPublication
        [HttpGet]
        public ActionResult<IEnumerable<EncouragementPublication>> GetEncouragementPublications()
        {
            return _context.EncouragementPublications
                           .Include(e => e.Publication)
                           .Include(e => e.Utilisateur)
                           .ToArray();
        }

        // GET: ApiSportTogether/EncouragementPublication/5
        [HttpGet("{id}")]
        public ActionResult<EncouragementPublication> GetEncouragementPublication(int id)
        {
            var encouragement = _context.EncouragementPublications
                                        .Include(e => e.Publication)
                                        .Include(e => e.Utilisateur)
                                        .FirstOrDefault(e => e.EncouragementPublicationId == id);

            if (encouragement == null)
            {
                return NotFound("Encouragement non trouvé.");
            }

            return Ok(encouragement);
        }

        // GET: ApiSportTogether/EncouragementPublication/ByPublication/5
        [HttpGet("ByPublication/{publicationId}")]
        public ActionResult<IEnumerable<EncouragementPublication>> GetEncouragementsByPublicationId(int publicationId)
        {
            var encouragements = _context.EncouragementPublications
                                         .Where(e => e.PublicationId == publicationId)
                                         .Include(e => e.Publication)
                                         .Include(e => e.Utilisateur)
                                         .ToArray();

            if (encouragements == null || !encouragements.Any())
            {
                return NotFound("Aucun encouragement trouvé pour cette publication.");
            }

            return Ok(encouragements);
        }

        // GET: ApiSportTogether/EncouragementPublication/ByUtilisateur/5
        [HttpGet("ByUtilisateur/{utilisateurId}")]
        public ActionResult<IEnumerable<EncouragementPublication>> GetEncouragementsByUtilisateurId(int utilisateurId)
        {
            var encouragements = _context.EncouragementPublications
                                         .Where(e => e.UtilisateurId == utilisateurId)
                                         .Include(e => e.Publication)
                                         .Include(e => e.Utilisateur)
                                         .ToArray();

            if (encouragements == null || !encouragements.Any())
            {
                return NotFound("Aucun encouragement trouvé pour cet utilisateur.");
            }

            return Ok(encouragements);
        }

        // POST: ApiSportTogether/EncouragementPublication/CreateEncouragement
        [HttpPost("CreateEncouragement")]
        public ActionResult<EncouragementPublication> CreateEncouragement([FromBody] EncouragementPublication encouragement)
        {
            if (encouragement == null)
            {
                return BadRequest("Données invalides.");
            }
            Publication? pb = _context.Publications.Find(encouragement.PublicationId);
            _context.EncouragementPublications.Add(encouragement);
            _context.SaveChanges();
            if (pb != null)
            {
                pb.NombreEncouragement += 1;
                _context.Entry(pb).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return CreatedAtAction(nameof(GetEncouragementPublication), new { id = encouragement.EncouragementPublicationId }, encouragement);
        }

        // PUT: ApiSportTogether/EncouragementPublication/5
        [HttpPut("{id}")]
        public IActionResult UpdateEncouragement(int id, [FromBody] EncouragementPublication encouragement)
        {
            if (id != encouragement.EncouragementPublicationId)
            {
                return BadRequest();
            }
           
            _context.Entry(encouragement).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
                
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.EncouragementPublications.Any(e => e.EncouragementPublicationId == id))
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

        // DELETE: ApiSportTogether/EncouragementPublication/5
        [HttpDelete("DeleteEncouragement/{publicationId}/{utilisateurId}")]
        public IActionResult DeleteEncouragement(int publicationId, int utilisateurId)
        {
            var encouragement = _context.EncouragementPublications.Where(ep => ep.PublicationId == publicationId && ep.UtilisateurId == utilisateurId).FirstOrDefault();
            if (encouragement == null)
            {
                return NotFound("Encouragement non trouvé.");
            }
            Publication? pb = _context.Publications.Find(publicationId);
            _context.EncouragementPublications.Remove(encouragement);
            _context.SaveChanges();
            if(pb != null)
            {
                pb.NombreEncouragement -= 1;
                _context.Entry(pb).State = EntityState.Modified;
                _context.SaveChanges();
            }
           

            return NoContent();
        }
    }
}
