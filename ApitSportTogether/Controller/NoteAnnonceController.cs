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
    public class NoteAnnonceController : ControllerBase
    {
        private readonly SportTogetherContext _context;

        public NoteAnnonceController(SportTogetherContext context)
        {
            _context = context;
        }

        // GET: ApiSportTogether/NoteAnnonce
        [HttpGet]
        public ActionResult<IEnumerable<NoteAnnonce>> GetNotes()
        {
            return _context.NoteAnnonces
                           .Include(n => n.Annonce) // Inclure les annonces associées
                           .Include(n => n.Utilisateur) // Inclure les utilisateurs associés
                           .ToArray();
        }

        // GET: ApiSportTogether/NoteAnnonce/5
        [HttpGet("{id}")]
        public ActionResult<NoteAnnonce> GetNoteById(int id)
        {
            var note = _context.NoteAnnonces
                               .Include(n => n.Annonce)
                               .Include(n => n.Utilisateur)
                               .FirstOrDefault(n => n.NoteAnnonceId == id);

            return note == null ? NotFound() : note;
        }

        // GET: ApiSportTogether/NoteAnnonce/GetByUserId/5
        [HttpGet("GetByUserId/{userId}")]
        public ActionResult<IEnumerable<NoteAnnonce>> GetNotesByUserId(int userId)
        {
            var notes = _context.NoteAnnonces
                                .Where(n => n.UtilisateurId == userId)
                                .Include(n => n.Annonce)
                                .ToArray();

            return notes.Any() ? Ok(notes) : NotFound();
        }

        // GET: ApiSportTogether/NoteAnnonce/GetByAnnonceId/5
        [HttpGet("GetByAnnonceId/{annonceId}")]
        public ActionResult<IEnumerable<NoteAnnonce>> GetNotesByAnnonceId(int annonceId)
        {
            var notes = _context.NoteAnnonces
                                .Where(n => n.AnnonceId == annonceId)
                                .Include(n => n.Utilisateur)
                                .ToArray();

            return notes.Any() ? Ok(notes) : NotFound();
        }

        // POST: ApiSportTogether/NoteAnnonce/CreateNote
        [HttpPost("CreateNote")]
        public ActionResult<NoteAnnonce> PostNote([FromBody] NoteAnnonce noteAnnonce)
        {
            if (noteAnnonce == null)
            {
                return BadRequest();
            }

            _context.NoteAnnonces.Add(noteAnnonce);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetNoteById), new { id = noteAnnonce.NoteAnnonceId }, noteAnnonce);
        }

        // PUT: ApiSportTogether/NoteAnnonce/5
        [HttpPut("{id}")]
        public ActionResult PutNote(int id, NoteAnnonce noteAnnonce)
        {
            if (id != noteAnnonce.NoteAnnonceId)
            {
                return BadRequest();
            }

            _context.Entry(noteAnnonce).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.NoteAnnonces.Any(n => n.NoteAnnonceId == id))
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

        // DELETE: ApiSportTogether/NoteAnnonce/5
        [HttpDelete("{id}")]
        public IActionResult DeleteNote(int id)
        {
            var note = _context.NoteAnnonces.Find(id);
            if (note == null)
            {
                return NotFound();
            }

            _context.NoteAnnonces.Remove(note);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
