namespace ApiSportTogether.Controller
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
    using global::ApiSportTogether.model.dbContext;
    using global::ApiSportTogether.model.ObjectContext;

    namespace ApiSportTogether.Controllers
    {
        [ApiController]
        [Route("[controller]")]
        public class SportFavoriController : ControllerBase
        {
            private readonly SportTogetherContext _context;

            public SportFavoriController(SportTogetherContext context)
            {
                _context = context;
            }

            // GET: SportFavori
            [HttpGet]
            public ActionResult<List<SportFavori>> GetSportsFavoris()
            {
                return  _context.SportFavoris
                    .Include(sf => sf.Sports)
                    .Include(sf => sf.Utilisateurs)
                    .ToList();
            }

            // GET: SportFavori/5
            [HttpGet("{id}")]
            public ActionResult<SportFavori> GetSportFavori(int id)
            {
                var sportFavori =  _context.SportFavoris
                    .Include(sf => sf.Sports)
                    .Include(sf => sf.Utilisateurs)
                    .FirstOrDefaultAsync(sf => sf.SportFavoriId == id);

                if (sportFavori == null)
                {
                    return NotFound();
                }

                return Ok(sportFavori);
            }

            // POST: SportFavori
            [HttpPost]
            public ActionResult<SportFavori> PostSportFavori(SportFavori sportFavori)
            {
                _context.SportFavoris.Add(sportFavori);
                 _context.SaveChangesAsync();

                return CreatedAtAction("GetSportFavori", new { id = sportFavori.SportFavoriId }, sportFavori);
            }

            // PUT: SportFavori/5
            [HttpPut("{id}")]
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
            public  ActionResult DeleteSportFavori(int id)
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

}
