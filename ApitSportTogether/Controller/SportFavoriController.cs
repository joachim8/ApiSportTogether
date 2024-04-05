using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.ObjectContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiSportTogether.Controller
{
    [ApiController]
    [Route("ApiSportTogether/[controller]")]
    public class SportFavoriController : ControllerBase
    {
        private readonly SportTogetherContext _context;

        public SportFavoriController(SportTogetherContext context)
        {
            _context = context;
        }

        // GET: ApiSportTogether/SportFavori
        [HttpGet]
        public ActionResult<List<SportFavori>> GetSportsFavoris()
        {
            return _context.SportFavoris
                .Include(sf => sf.Sports)
                .Include(sf => sf.Utilisateurs)
                .ToList();
        }

        // GET: ApiSportTogether/SportFavori/5
        [HttpGet("{id}")]
        public ActionResult<SportFavori> GetSportFavori(int id)
        {
            var sportFavori = _context.SportFavoris
                .Include(sf => sf.Sports)
                .Include(sf => sf.Utilisateurs)
                .FirstOrDefaultAsync(sf => sf.SportFavoriId == id);

            if (sportFavori == null)
            {
                return NotFound();
            }

            return Ok(sportFavori);
        }

        // POST: ApiSportTogether/SportFavori/CreateSportFavori
        [HttpPost("CreateSportFavori")]
        public ActionResult<SportFavori> PostSportFavori([FromBody]SportFavori sportFavori)
        {
            if (sportFavori != null)
            {
                _context.SportFavoris.Add(sportFavori);
                _context.SaveChanges();

                return CreatedAtAction("GetSportFavori", new { id = sportFavori.SportFavoriId }, sportFavori);
            }
            else
            {
                return NotFound();
            }
        }

        // PUT: ApiSportTogether/SportFavori/UpdateSportFavori/5
        [HttpPut("UpdateSportFavori/{id}")]
        public ActionResult<SportFavori> PutSportFavori(int id, SportFavori sportFavori)
        {
            if (id != sportFavori.SportFavoriId)
            {
                return BadRequest();
            }

            _context.Entry(sportFavori).State = EntityState.Modified;

            try
            {
                _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SportFavoriExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(sportFavori);
        }

        // DELETE: SportFavori/5
        [HttpDelete("{id}")]
        public ActionResult DeleteSportFavori(int id)
        {
            var sportFavori = _context.SportFavoris.Find(id);
            if (sportFavori == null)
            {
                return NotFound();
            }

            _context.SportFavoris.Remove(sportFavori);
            _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SportFavoriExists(int id)
        {
            return _context.SportFavoris.Any(e => e.SportFavoriId == id);
        }
    }
}

