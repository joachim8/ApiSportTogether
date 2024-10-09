using ApiSportTogether.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiSportTogether.Controller
{
    [ApiController]
    [Route("ApiSportTogether/[controller]")]
    public class NotificationBackgroundController : ControllerBase
    {
        private readonly NotificationService _notificationService;

        public NotificationBackgroundController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // Endpoint pour déclencher manuellement l'envoi des notifications
        [HttpPost("notify-participants")]
        public async Task<IActionResult> NotifyParticipants()
        {
            await _notificationService.NotifierParticipantsAsync();
            return Ok("Notifications envoyées avec succès.");
        }
    }
}
