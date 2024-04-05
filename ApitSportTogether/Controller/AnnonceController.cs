using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.ObjectContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiSportTogether.Controller
{
    [ApiController]
    [Route("ApiSportTogether/[controller]")]
    public class AnnonceController : ControllerBase
    {
        private readonly SportTogetherContext _context;
        private readonly IConfiguration _configuration;

        public AnnonceController(IConfiguration configuration, SportTogetherContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: ApiSportTogether/Annonce
        [HttpGet]
        public ActionResult<List<Annonce>> GetAnnonces()
        {
            return _context.Annonces
                           .Include(a => a.AuteurNavigation)
                           .Include(a => a.Sport)
                           .Include(a => a.Image)
                           .ToList();
        }

        // GET: ApiSportTogether/Annonce/5
        [HttpGet("{id}")]
        public ActionResult<Annonce> GetAnnonceById(int id)
        {
            var annonce = _context.Annonces
                                  .Include(a => a.AuteurNavigation)
                                  .Include(a => a.Sport)
                                  .Include(a => a.Image)
                                  .FirstOrDefault(a => a.AnnoncesId == id);

            return annonce == null ? NotFound() : annonce;
        }

        // POST: ApiSportTogether/Annonce
        [HttpPost]
        public ActionResult<Annonce> PostAnnonce([FromBody] Annonce annonce)
        {
            _context.Annonces.Add(annonce);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAnnonceById), new { id = annonce.AnnoncesId }, annonce);
        }

        // PUT: ApiSportTogether/Annonce/5
        [HttpPut("{id}")]
        public IActionResult PutAnnonce(int id, Annonce annonce)
        {
            if (id != annonce.AnnoncesId)
            {
                return BadRequest();
            }

            _context.Entry(annonce).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Annonces.Any(a => a.AnnoncesId == id))
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

        // DELETE: ApiSportTogether/Annonce/5
        [HttpDelete("{id}")]
        public IActionResult DeleteAnnonce(int id)
        {
            var annonce = _context.Annonces.Find(id);
            if (annonce == null)
            {
                return NotFound();
            }

            _context.Annonces.Remove(annonce);
            _context.SaveChanges();

            return NoContent();
        }
    }
}

