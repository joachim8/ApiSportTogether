using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.ObjectContext;
using System.Collections.Generic;
using System.Linq;

namespace ApiSportTogether.Controller
{
    [ApiController]
    [Route("ApiSportTogether/[controller]")]
    public class NotificationUtilisateurController : ControllerBase
    {
        private readonly SportTogetherContext _context;

        public NotificationUtilisateurController(SportTogetherContext context)
        {
            _context = context;
        }

        // GET: ApiSportTogether/NotificationUtilisateur
        [HttpGet]
        public ActionResult<IEnumerable<NotificationUtilisateur>> GetNotifications()
        {
            return _context.NotificationUtilisateurs
                           .Include(n => n.Utilisateur)
                           .ToArray();
        }

        // GET: ApiSportTogether/NotificationUtilisateur/5
        [HttpGet("{id}")]
        public ActionResult<NotificationUtilisateur> GetNotificationById(int id)
        {
            var notification = _context.NotificationUtilisateurs
                                        .Include(n => n.Utilisateur)
                                        .FirstOrDefault(n => n.NotificationId == id);

            return notification == null ? NotFound() : notification;
        }

        // GET: ApiSportTogether/NotificationUtilisateur/GetByUserId/5
        [HttpGet("GetByUserId/{userId}")]
        public ActionResult<IEnumerable<NotificationUtilisateur>> GetNotificationsByUserId(int userId)
        {
            var notifications = _context.NotificationUtilisateurs
                                        .Where(n => n.UtilisateurId == userId)
                                        .Include(n => n.Utilisateur)
                                        .ToArray();

            return notifications.Any() ? Ok(notifications) : NotFound();
        }

        // POST: ApiSportTogether/NotificationUtilisateur/CreateNotification
        [HttpPost("CreateNotification")]
        public ActionResult<NotificationUtilisateur> PostNotification([FromBody] NotificationUtilisateur notification)
        {
            if (notification == null)
            {
                return BadRequest();
            }

            _context.NotificationUtilisateurs.Add(notification);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetNotificationById), new { id = notification.NotificationId }, notification);
        }

        // PUT: ApiSportTogether/NotificationUtilisateur/5
        [HttpPut("{id}")]
        public ActionResult PutNotification(int id, NotificationUtilisateur notification)
        {
            if (id != notification.NotificationId)
            {
                return BadRequest();
            }

            _context.Entry(notification).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.NotificationUtilisateurs.Any(n => n.NotificationId == id))
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

        // DELETE: ApiSportTogether/NotificationUtilisateur/5
        [HttpDelete("{id}")]
        public IActionResult DeleteNotification(int id)
        {
            var notification = _context.NotificationUtilisateurs.Find(id);
            if (notification == null)
            {
                return NotFound();
            }

            _context.NotificationUtilisateurs.Remove(notification);
            _context.SaveChanges();

            return NoContent();
        }

        // PATCH: ApiSportTogether/NotificationUtilisateur/MarkAsRead/5
        [HttpPatch("MarkAsRead/{id}")]
        public ActionResult MarkNotificationAsRead(int id)
        {
            var notification = _context.NotificationUtilisateurs.Find(id);
            if (notification == null)
            {
                return NotFound();
            }

            notification.Vu = true;
            _context.Entry(notification).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }
    }
}

