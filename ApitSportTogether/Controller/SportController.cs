using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.ObjectContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiSportTogether.Controller
{
    [ApiController]
    [Route("ApiSportTogether/[controller]")]
    public class SportController : ControllerBase
    {
        private readonly SportTogetherContext _context;

        public SportController(SportTogetherContext context)
        {
            _context = context;
        }

        // GET: ApiSportTogether/Sport
        [HttpGet]
        public ActionResult<List<Sport>> GetSports()
        {
            List<Sport> sports = new List<Sport>();
            sports = _context.Sports.ToList();
            return sports;
        }

        // GET: ApiSportTogether/Sport/5
        [HttpGet("{id}")]
        public ActionResult<Sport> GetSportById(int id)
        {
            var sport = _context.Sports.Find(id);
            return sport == null ? NotFound() : sport;
        }

        // POST: ApiSportTogether/Sport
        [HttpPost]
        public ActionResult<Sport> PostSport([FromBody] Sport sport)
        {
            if (sport != null)
            {
                _context.Sports.Add(sport);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetSportById), new { id = sport.SportsId }, sport);
            }
            else
            {
                return NotFound();
            }
        }

        // PUT: ApiSportTogether/Sport/5
        [HttpPut("{id}")]
        public IActionResult PutSport(int id, Sport sport)
        {
            if (id != sport.SportsId)
            {
                return BadRequest();
            }

            _context.Entry(sport).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Sports.Any(s => s.SportsId == id))
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

        // DELETE: ApiSportTogether/Sport/5
        [HttpDelete("{id}")]
        public IActionResult DeleteSport(int id)
        {
            var sport = _context.Sports.Find(id);
            if (sport == null)
            {
                return NotFound();
            }

            _context.Sports.Remove(sport);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
