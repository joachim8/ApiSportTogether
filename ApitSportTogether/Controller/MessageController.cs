using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.ObjectContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiSportTogether.Controller
{
    [ApiController]
    [Route("ApiSportTogether/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly SportTogetherContext _context;
        private readonly IConfiguration _configuration;

        public MessageController(IConfiguration configuration, SportTogetherContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: ApiSportTogether/Message
        [HttpGet]
        public ActionResult<List<Message>> GetMessages()
        {
            return _context.Messages
                           .Include(m => m.VuMessages)
                           .ToList();
        }

        // GET: ApiSportTogether/Message/5
        [HttpGet("{id}")]
        public ActionResult<Message> GetMessageById(int id)
        {
            var message = _context.Messages
                                  .Include(m => m.VuMessages)
                                  .FirstOrDefault(m => m.MessagesId == id);

            return message == null ? NotFound() : message;
        }

        // POST: ApiSportTogether/Message
        [HttpPost]
        public ActionResult<Message> PostMessage([FromBody] Message message)
        {
            if( message == null) return NotFound();
            if(message.GroupeId == 0) return NotFound();
            Groupe groupe = _context.Groupes.Include(g => g.MembreGroupes).Where(g => g.GroupesId == message.GroupeId).First()!;
            if( groupe == null ) return NotFound();
            Message messageApresEnregistrement = new();


            _context.Messages.Add(message);
            
            groupe.LastMessage = message.Contenu;
            _context.Entry(groupe).State = EntityState.Modified;
            foreach (MembreGroupe mg in groupe.MembreGroupes)
            {
                if(mg.UtilisateurId != message.UtilisateurId)
                {
                    VuMessage vuMessage = new VuMessage()
                    {
                        IdMessage = message.MessagesId,
                        UtilisateurId = mg.UtilisateurId,
                        Vu = false
                    };
                    _context.VuMessages.Add(vuMessage);

                    
                }
                

            }
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetMessageById), new { id = message.MessagesId }, message);
        }

        // PUT: ApiSportTogether/Message/5
        [HttpPut("{id}")]
        public IActionResult PutMessage(int id, Message message)
        {
            if (id != message.MessagesId)
            {
                return BadRequest();
            }

            _context.Entry(message).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Messages.Any(m => m.MessagesId == id))
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

        // DELETE: ApiSportTogether/Message/5
        [HttpDelete("{id}")]
        public IActionResult DeleteMessage(int id)
        {
            var message = _context.Messages.Find(id);
            if (message == null)
            {
                return NotFound();
            }

            _context.Messages.Remove(message);
            _context.SaveChanges();

            return NoContent();
        }

        // GET: ApiSportTogether/Message/GetMessagesByGroupe/{GroupeId}
        [HttpGet("GetMessagesByGroupe/{GroupeId}")]
        public ActionResult<IEnumerable<Message>> GetMessagesByGroupe(int GroupeId)
        {
            var messages = _context.Messages
                                   .Where(m => m.GroupeId == GroupeId)
                                   .OrderBy(m => m.Timestamp)
                                   .ToList();

            return !messages.Any() ? NoContent() : messages.ToArray();
        }
    }
}
