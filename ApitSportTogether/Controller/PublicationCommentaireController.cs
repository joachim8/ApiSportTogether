using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.ObjectContext;
using ApiSportTogether.model.ObjectVue;
using Microsoft.AspNetCore.Mvc;
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

        public PublicationCommentaireController(SportTogetherContext context)
        {
            _context = context;
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
        public ActionResult<PublicationCommentaire> PostPublicationCommentaire([FromBody] PublicationCommentaire commentaire)
        {
            if (commentaire == null)
            {
                return BadRequest();
            }

            _context.PublicationCommentaires.Add(commentaire);
            _context.SaveChanges();

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
        public IActionResult DeletePublicationCommentaire(int id)
        {
            var commentaire = _context.PublicationCommentaires.Find(id);
            if (commentaire == null)
            {
                return NotFound();
            }

            _context.PublicationCommentaires.Remove(commentaire);
            _context.SaveChanges();

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
    }
}
