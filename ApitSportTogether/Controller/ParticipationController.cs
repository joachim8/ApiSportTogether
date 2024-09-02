using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.ObjectContext;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ApiSportTogether.Controller
{
    [ApiController]
    [Route("ApiSportTogether/[controller]")]
    public class ParticipationController : ControllerBase
    {
        private readonly SportTogetherContext _context;
        private readonly IConfiguration _configuration;

        public ParticipationController(IConfiguration configuration, SportTogetherContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: ApiSportTogether/Participation/Annonce/5/count
        [HttpGet("Annonce/{annonceId}/count")]
        public ActionResult<int> GetParticipationCountByAnnonceId(int annonceId)
        {
            var count = _context.Participations.Count(p => p.AnnonceId == annonceId);
            return Ok(count);
        }

        // GET: ApiSportTogether/Participation/Annonce/5
        [HttpGet("Annonce/{annonceId}")]
        public ActionResult<IEnumerable<Participation>> GetParticipationsByAnnonceId(int annonceId)
        {
            var participations = _context.Participations
                                         .Where(p => p.AnnonceId == annonceId)
                                         .ToList();
            return participations.Count == 0 ? NotFound() : Ok(participations);
        }

        // GET: ApiSportTogether/Participation/Utilisateur/5
        [HttpGet("Utilisateur/{utilisateurId}")]
        public ActionResult<IEnumerable<Participation>> GetParticipationsByUtilisateurId(int utilisateurId)
        {
            var participations = _context.Participations
                                         .Where(p => p.UtilisateurId == utilisateurId)
                                         .ToList();
            return participations.Count == 0 ? NotFound() : Ok(participations);
        }

        // GET: ApiSportTogether/Participation/CheckParticipation
        [HttpGet("CheckParticipation/{utilisateurId}/{annonceId}")]
        public ActionResult<bool> CheckIfUserParticipates(int utilisateurId, int annonceId)
        {
            var participation = _context.Participations
                                        .Any(p => p.UtilisateurId == utilisateurId && p.AnnonceId == annonceId);
            return Ok(participation);
        }
        // POST: ApiSportTogether/Participation/CreateParticipation
        [HttpPost("CreateParticipation")]
        public ActionResult<Participation> PostParticipation([FromBody] Participation participation)
        {
            if (participation == null)
            {
                return BadRequest("La participation ne peut pas être nulle.");
            }

            var annonce = _context.Annonces.Find(participation.AnnonceId);
            var groupe = _context.Groupes.Where(g => g.AnnonceId == participation.AnnonceId).FirstOrDefault();

            if (annonce == null)
            {
                return NotFound("Annonce non trouvée.");
            }

            var participantCount = _context.Participations.Count(p => p.AnnonceId == participation.AnnonceId);
            if (participantCount >= annonce.NombreParticipants)
            {
                return BadRequest("Le nombre maximum de participants est atteint.");
            }
            participation.GroupeId = groupe.GroupesId;
            _context.Participations.Add(participation);
            MembreGroupe mg = new()
            {
                GroupeId = (int)participation.GroupeId,
                Role = "Admin",
                UtilisateurId = (int)participation.UtilisateurId
            };
            _context.MembreGroupes.Add(mg);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetParticipationsByAnnonceId), new { annonceId = participation.AnnonceId }, participation);
        }
        // POST: ApiSportTogether/Participation/DeleteParticipation
        [HttpPost("DeleteParticipation")]
        public IActionResult DeleteParticipation([FromBody] Participation participation)
        {
            if (participation == null)
            {
                return BadRequest("La participation ne peut pas être nulle.");
            }

            Participation participationtrouver =  _context.Participations.Where(p => p.AnnonceId == participation.AnnonceId && p.UtilisateurId == participation.UtilisateurId).FirstOrDefault()!;
           
 
            if (participationtrouver == null)
            {
                return NotFound("Participation non trouvée.");
            }

            _context.Participations.Remove(participationtrouver);
            _context.SaveChanges();

            return NoContent();
        }
    }
}


