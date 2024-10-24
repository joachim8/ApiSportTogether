﻿using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.ObjectContext;
using ApiSportTogether.Services;
using ApiSportTogether.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ApiSportTogether.Controller
{
    [ApiController]
    [Route("ApiSportTogether/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly SportTogetherContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHubContext<ChatHubSportTogether> _hubContext;

        public MessageController(IConfiguration configuration, SportTogetherContext context, IHubContext<ChatHubSportTogether> hubContext)
        {
            _context = context;
            _configuration = configuration;
            _hubContext = hubContext;
        }

        // GET: ApiSportTogether/Message
        [HttpGet]
        public ActionResult<IEnumerable<Message>> GetMessages()
        {
            return _context.Messages
                           .Include(m => m.VuMessages)
                           .ToArray();
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

        // POST: ApiSportTogether/Message/CreateMessage
        [HttpPost("CreateMessage")]
        public ActionResult<Message> PostMessage([FromBody] Message message)
        {
            if( message == null) return NotFound();
            if(message.GroupeId == 0) return NotFound();
            Groupe groupe = _context.Groupes.Include(g => g.MembreGroupes).Where(g => g.GroupesId == message.GroupeId).First()!;
            if( groupe == null ) return NotFound();
            // Créer une instance de VerificateurDeTexte
            VerificateurDeTexte verificateurDeTexte = new VerificateurDeTexte();
            // Vérifier le texte pour des mots racistes ou sexistes
            var (isClean, motsTrouves) = verificateurDeTexte.VerifierTexte(message.Contenu!);

            if (!isClean)
            {
                return BadRequest(new
                {
                    Message = "Le texte contient des mots interdits.",
                    MotsTrouves = motsTrouves
                });
            }

            message.urlProfilImage = _context.ProfileImages.Where(pi => pi.UtilisateursId == message.UtilisateurId).FirstOrDefault()!.Url!;
            _context.Messages.Add(message);
            _context.SaveChanges();
            // Récupérer le groupe pour envoyer la notification
            string? urlPhoto = _context.ProfileImages.Where(pi => pi.UtilisateursId == message.UtilisateurId).FirstOrDefault()?.Url!;
            if (groupe != null && urlPhoto != null)
            {
                // Envoyer une notification à tous les membres du groupe via SignalR
                _hubContext.Clients.Group($"{groupe.GroupesId}¤{groupe.Nom}").SendAsync("ReceiveMessage", message.NomUtilisateur, $"{ message.Contenu}¤{ urlPhoto}¤{groupe.Nom}");
            }
            groupe.LastMessage = message.Contenu;
            _context.Entry(groupe).State = EntityState.Modified;
            
            foreach (MembreGroupe mg in groupe.MembreGroupes)
            {
                if(mg.UtilisateurId != message.UtilisateurId)
                {
                    VuMessage vuMessage = new VuMessage()
                    {
                        messages_id = message.MessagesId,
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
            Groupe? groupe = null;
            string? user = null;
            if (message == null)
            {
                return NotFound();
            }
            else
            {
                user = message.NomUtilisateur;
                groupe = _context.Groupes.Where(g => g.GroupesId == message.GroupeId).FirstOrDefault();
                List<VuMessage> vuMessages = _context.VuMessages.Where(vm => vm.messages_id == message.MessagesId).ToList();
                if(vuMessages.Any())
                {
                    foreach(VuMessage vm in vuMessages)
                    {
                        _context.VuMessages.Remove(vm);
                        _context.SaveChanges();
                    }
                }
            }

            _context.Messages.Remove(message);
            _context.SaveChanges();
            if(groupe != null)
            {
                // Notifier les autres clients via SignalR
                 _hubContext.Clients.Group($"{groupe.GroupesId}¤{groupe.Nom}").SendAsync("SupprimerMessageClient", user,id);
            }

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
