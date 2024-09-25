using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.ObjectContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ApiSportTogether.Controller
{
    [ApiController]
    [Route("ApiSportTogether/[controller]")]
    public class EncouragementPublicationCommentaireController : ControllerBase
    {
        private readonly SportTogetherContext _context;


        public EncouragementPublicationCommentaireController(SportTogetherContext context)
        {
            _context = context;
        }

        // GET: ApiSportTogether/EncouragementPublicationCommentaire
        [HttpGet]
        public ActionResult<IEnumerable<EncouragementPublicationCommentaire>> GetEncouragementPublicationCommentaires()
        {
            var encouragements = _context.EncouragementPublicationCommentaires
                                         .Include(e => e.PublicationCommentaire)
                                         .Include(e => e.Utilisateur)
                                         .ToArray();
            return Ok(encouragements);
        }

        // GET: ApiSportTogether/EncouragementPublicationCommentaire/{id}
        [HttpGet("{id}")]
        public ActionResult<EncouragementPublicationCommentaire> GetEncouragementPublicationCommentaireById(int id)
        {
            var encouragement = _context.EncouragementPublicationCommentaires
                                        .Include(e => e.PublicationCommentaire)
                                        .Include(e => e.Utilisateur)
                                        .FirstOrDefault(e => e.EncouragementPublicationCommentaireId == id);

            if (encouragement == null)
            {
                return NotFound("Encouragement non trouvé.");
            }

            return Ok(encouragement);
        }

        // GET: ApiSportTogether/EncouragementPublicationCommentaire/ByPublicationCommentaire/{publicationCommentaireId}
        [HttpGet("ByPublicationCommentaire/{publicationCommentaireId}")]
        public ActionResult<IEnumerable<EncouragementPublicationCommentaire>> GetByPublicationCommentaireId(int publicationCommentaireId)
        {
            var encouragements = _context.EncouragementPublicationCommentaires
                                         .Where(e => e.PublicationCommentaireId == publicationCommentaireId)
                                         .Include(e => e.PublicationCommentaire)
                                         .Include(e => e.Utilisateur)
                                         .ToArray();
            return Ok(encouragements);
        }

        // GET: ApiSportTogether/EncouragementPublicationCommentaire/ByUtilisateur/{utilisateurId}
        [HttpGet("ByUtilisateur/{utilisateurId}")]
        public ActionResult<IEnumerable<EncouragementPublicationCommentaire>> GetByUtilisateurId(int utilisateurId)
        {
            var encouragements = _context.EncouragementPublicationCommentaires
                                         .Where(e => e.UtilisateurId == utilisateurId)
                                         .Include(e => e.PublicationCommentaire)
                                         .Include(e => e.Utilisateur)
                                         .ToArray();
            return Ok(encouragements);
        }

        // POST: ApiSportTogether/EncouragementPublicationCommentaire/CreateEncouragementPublicationCommentaire
        [HttpPost("CreateEncouragementPublicationCommentaire")]
        public ActionResult<EncouragementPublicationCommentaire> CreateEncouragementPublicationCommentaire([FromBody] EncouragementPublicationCommentaire encouragement)
        {
            if (encouragement == null)
            {
                return BadRequest("Les données sont invalides.");
            }
            PublicationCommentaire? publicationCommentaire = _context.PublicationCommentaires.Find(encouragement.PublicationCommentaireId);
            _context.EncouragementPublicationCommentaires.Add(encouragement);
            _context.SaveChanges();
            if(publicationCommentaire != null)
            {
                publicationCommentaire.NombreEncouragementCommentaire += 1;
                _context.Entry(publicationCommentaire).State = EntityState.Modified;
                _context.SaveChanges();
            }

            return CreatedAtAction(nameof(GetEncouragementPublicationCommentaireById), new { id = encouragement.EncouragementPublicationCommentaireId }, encouragement);
        }

        // PUT: ApiSportTogether/EncouragementPublicationCommentaire/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateEncouragementPublicationCommentaire(int id, [FromBody] EncouragementPublicationCommentaire encouragement)
        {
            if (id != encouragement.EncouragementPublicationCommentaireId)
            {
                return BadRequest("L'ID de l'encouragement ne correspond pas.");
            }

            _context.Entry(encouragement).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.EncouragementPublicationCommentaires.Any(e => e.EncouragementPublicationCommentaireId == id))
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

        // DELETE: ApiSportTogether/EncouragementPublicationCommentaire/{id}
        [HttpDelete("DeleteEncouragementPublicationCommentaire/{PublicationCommentaireId}/{utilisateurId}")]
        public ActionResult DeleteEncouragementPublicationCommentaire(int publicationCommentaireId, int utilisateurId)
        {
            var encouragement = _context.EncouragementPublicationCommentaires.Where(epc => epc.PublicationCommentaireId == publicationCommentaireId && epc.UtilisateurId == utilisateurId).FirstOrDefault();
            if (encouragement == null)
            {
                return NotFound();
            }
            PublicationCommentaire? publicationCommentaire = _context.PublicationCommentaires.Find(publicationCommentaireId);
            _context.EncouragementPublicationCommentaires.Remove(encouragement);
            _context.SaveChanges();
            if (publicationCommentaire != null)
            {
                publicationCommentaire.NombreEncouragementCommentaire -= 1;
                _context.Entry(publicationCommentaire).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return NoContent();
        }
    }
}
