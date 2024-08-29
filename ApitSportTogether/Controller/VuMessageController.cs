using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.ObjectContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiSportTogether.Controller
{
    [ApiController]
    [Route("ApiSportTogether/[controller]")]
    public class VuMessageController : ControllerBase
    {
        private readonly SportTogetherContext _context;
        private readonly IConfiguration _configuration;

        public VuMessageController(IConfiguration configuration, SportTogetherContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: ApiSportTogether/VuMessage
        [HttpGet]
        public ActionResult<List<VuMessage>> GetVuMessages()
        {
            return _context.VuMessages
                           .Include(vm => vm.IdMessageNavigation)
                           .Include(vm => vm.Utilisateur)
                           .ToList();
        }

        // GET: ApiSportTogether/VuMessage/5
        [HttpGet("{id}")]
        public ActionResult<VuMessage> GetVuMessageById(int id)
        {
            var vuMessage = _context.VuMessages
                                    .Include(vm => vm.IdMessageNavigation)
                                    .Include(vm => vm.Utilisateur)
                                    .FirstOrDefault(vm => vm.IdMessageVu == id);

            return vuMessage == null ? NotFound() : vuMessage;
        }

        // POST: ApiSportTogether/VuMessage
        [HttpPost]
        public ActionResult<VuMessage> PostVuMessage([FromBody] VuMessage vuMessage)
        {
            _context.VuMessages.Add(vuMessage);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetVuMessageById), new { id = vuMessage.IdMessageVu }, vuMessage);
        }

        // PUT: ApiSportTogether/VuMessage/5
        [HttpPut("{id}")]
        public IActionResult PutVuMessage(int id, VuMessage vuMessage)
        {
            if (id != vuMessage.IdMessageVu)
            {
                return BadRequest();
            }

            _context.Entry(vuMessage).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.VuMessages.Any(vm => vm.IdMessageVu == id))
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

        // DELETE: ApiSportTogether/VuMessage/5
        [HttpDelete("{id}")]
        public IActionResult DeleteVuMessage(int id)
        {
            var vuMessage = _context.VuMessages.Find(id);
            if (vuMessage == null)
            {
                return NotFound();
            }

            _context.VuMessages.Remove(vuMessage);
            _context.SaveChanges();

            return NoContent();
        }

        // GET: ApiSportTogether/VuMessage/GetVuMessagesByUtilisateur/5
        [HttpGet("GetVuMessagesByUtilisateur/{utilisateurId}")]
        public ActionResult<IEnumerable<VuMessage>> GetVuMessagesByUtilisateur(int utilisateurId)
        {
            var messagesVus = _context.VuMessages
                                      .Include(vm => vm.IdMessageNavigation)
                                      .Where(vm => vm.UtilisateurId == utilisateurId)
                                      .ToList();

            return !messagesVus.Any() ? NoContent() : Ok(messagesVus);
        }

        // GET: ApiSportTogether/VuMessage/GetVuMessagesByMessage/5
        [HttpGet("GetVuMessagesByMessage/{messageId}")]
        public ActionResult<IEnumerable<VuMessage>> GetVuMessagesByMessage(int messageId)
        {
            var messagesVus = _context.VuMessages
                                      .Include(vm => vm.Utilisateur)
                                      .Where(vm => vm.IdMessage == messageId)
                                      .ToList();

            return !messagesVus.Any() ? NoContent() : Ok(messagesVus);
        }
    }
}
