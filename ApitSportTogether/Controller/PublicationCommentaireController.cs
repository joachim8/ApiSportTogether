using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.ObjectContext;
using ApiSportTogether.model.ObjectVue;
using ApiSportTogether.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ApiSportTogether.Controller
{
    [ApiController]
    [Route("ApiSportTogether/[controller]")]
    public class PublicationCommentaireController : ControllerBase
    {
        private readonly SportTogetherContext _context;
        private readonly IHubContext<PublicationHub> _hubContext;
        public PublicationCommentaireController(SportTogetherContext context, IHubContext<PublicationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        // GET: ApiSportTogether/PublicationCommentaire
        [HttpGet]
        public ActionResult<List<PublicationCommentaire>> GetPublicationCommentaires()
        {
            return _context.PublicationCommentaires
                           .Include(c => c.Utilisateur)
                           .Include(c => c.Publication)
                           .ToList();
        }

        // GET: ApiSportTogether/PublicationCommentaire/5
        [HttpGet("{id}")]
        public ActionResult<PublicationCommentaire> GetPublicationCommentaireById(int id)
        {
            var commentaire = _context.PublicationCommentaires
                                       .Include(c => c.Utilisateur)
                                       .Include(c => c.Publication)
                                       .FirstOrDefault(c => c.CommentaireId == id);

            return commentaire == null ? NotFound() : commentaire;
        }

        // POST: ApiSportTogether/PublicationCommentaire/CreateCommentaire
        [HttpPost("CreateCommentaire")]
        public async Task<ActionResult<PublicationCommentaire>> PostPublicationCommentaire([FromBody] PublicationCommentaire commentaire)
        {
            if (commentaire == null)
            {
                return BadRequest();
            }

            _context.PublicationCommentaires.Add(commentaire);
            _context.SaveChanges();

            // Notifier via SignalR que le commentaire a été ajouté
            await _hubContext.Clients.Group(commentaire.PublicationId.ToString())
                .SendAsync("ReceiveCommentAdded", commentaire.CommentaireId);

            return CreatedAtAction(nameof(GetPublicationCommentaireById), new { id = commentaire.CommentaireId }, commentaire);
        }



        // PUT: ApiSportTogether/PublicationCommentaire/5
        [HttpPut("{id}")]
        public ActionResult PutPublicationCommentaire(int id, PublicationCommentaire commentaire)
        {
            if (id != commentaire.CommentaireId)
            {
                return BadRequest();
            }

            _context.Entry(commentaire).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.PublicationCommentaires.Any(c => c.CommentaireId == id))
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

        // DELETE: ApiSportTogether/PublicationCommentaire/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublicationCommentaire(int id)
        {
            var commentaire = _context.PublicationCommentaires.Find(id);
            if (commentaire == null)
            {
                return NotFound();
            }

            var commentaireId = commentaire.CommentaireId;
            var publicationId = commentaire.PublicationId;

            _context.PublicationCommentaires.Remove(commentaire);
            _context.SaveChanges();

            // Notifier via SignalR que le commentaire a été supprimé
            await _hubContext.Clients.Group(publicationId.ToString())
                .SendAsync("ReceiveCommentDeleted", commentaireId);

            return NoContent();
        }



        // GET: ApiSportTogether/PublicationCommentaire/GetCommentairesByPublicationId/5
        [HttpGet("GetCommentairesByPublicationId/{publicationId}")]
        public ActionResult<List<PublicationCommentaire>> GetCommentairesByPublicationId(int publicationId)
        {
            var commentaires = _context.PublicationCommentaires
                                        .Where(c => c.PublicationId == publicationId)
                                        .Include(c => c.Utilisateur)
                                        .ToList();

            return commentaires.Any() ? Ok(commentaires) : NotFound();
        }

        // GET: ApiSportTogether/PublicationCommentaire/GetCommentaireById/5
        [HttpGet("GetCommentaireById/{commentaireId:int}")]
        public ActionResult<CommentaireVue> GetCommentaireById(int commentaireId)
        {
            var commentaire = _context.PublicationCommentaires
                .Where(c => c.CommentaireId == commentaireId)
                .Include(c => c.Utilisateur)
                .Select(c => new CommentaireVue
                {
                    CommentaireId = c.CommentaireId,
                    UtilisateurId = c.UtilisateurId,
                    PseudoUtilisateur = c.Utilisateur.Pseudo, // Assurez-vous que le modèle Utilisateur a une propriété Pseudo
                    Contenu = c.Contenu,
                    DateCommentaire = c.DateCommentaire,
                    NombreEncouragementCommentaire = c.NombreEncouragementCommentaire,
                    ImageUtilisateurUrl = _context.ProfileImages.Where(pi => pi.UtilisateursId == c.UtilisateurId).FirstOrDefault()!.Url // Assurez-vous que le modèle Utilisateur a une propriété ImageUrl
                })
                .FirstOrDefault();

            if (commentaire == null)
            {
                return NotFound(); // 404 si aucun commentaire trouvé
            }

            return Ok(commentaire); // 200 avec le commentaire
        }
    }
}
