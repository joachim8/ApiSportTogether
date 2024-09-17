using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.ObjectContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiSportTogether.Controller
{
    [ApiController]
    [Route("ApiSportTogether/[controller]")]
    public class AmiController : ControllerBase
    {
        private readonly SportTogetherContext _context;
        private readonly IConfiguration _configuration;

        public AmiController(IConfiguration configuration, SportTogetherContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: ApiSportTogether/Ami
        [HttpGet]
        public ActionResult<IEnumerable<Ami>> GetAmis()
        {
            return _context.Amis.Include(a => a.UtilisateurId1Navigation)
                                .Include(a => a.UtilisateurId2Navigation).ToArray();
        }

        // GET: ApiSportTogether/Ami/5
        [HttpGet("{id}")]
        public ActionResult<Ami> GetAmiById(int id)
        {
            var ami = _context.Amis.Include(a => a.UtilisateurId1Navigation)
                                   .Include(a => a.UtilisateurId2Navigation)
                                   .FirstOrDefault(a => a.AmisId == id);

            return ami == null ? NotFound() : ami;
        }

        // POST: ApiSportTogether/Ami
        [HttpPost]
        public ActionResult<Ami> PostAmi([FromBody] Ami ami)
        {
            _context.Amis.Add(ami);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAmiById), new { id = ami.AmisId }, ami);
        }

        // PUT: ApiSportTogether/Ami/5
        [HttpPut("{id}")]
        public IActionResult PutAmi(int id, Ami ami)
        {
            if (id != ami.AmisId)
            {
                return BadRequest();
            }

            _context.Entry(ami).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Amis.Any(a => a.AmisId == id))
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

        // DELETE: ApiSportTogether/Ami/5
        [HttpDelete("{id}")]
        public IActionResult DeleteAmi(int id)
        {
            var ami = _context.Amis.Find(id);
            if (ami == null)
            {
                return NotFound();
            }

            _context.Amis.Remove(ami);
            _context.SaveChanges();

            return NoContent();
        }
        // GET: ApiSportTogether/Ami/GetNombreAmi/1
        [HttpGet("GetNombreAmi/{utilisateur_id}")]
        public ActionResult<int> GetNombreAmi(int utilisateur_id)
        {
            if (utilisateur_id == 0) return NotFound(); 
            int nbrAmis = _context.Amis.Where(a => a.UtilisateurId1 == utilisateur_id || a.UtilisateurId2 == utilisateur_id).Count();
            return nbrAmis;
        }
        // GET: ApiSportTogether/Ami/GetListAmi/1
        [HttpGet("GetListAmi/{utilisateur_id}")]
        public ActionResult<IEnumerable<Utilisateur>> GetListAmi(int utilisateur_id)
        {
            if (utilisateur_id == 0)
            {
                return NotFound();
            }

            // Récupérer la liste des relations d'amitié impliquant l'utilisateur
            List<Ami> listAmi = _context.Amis
                                        .Where(a => a.UtilisateurId1 == utilisateur_id || a.UtilisateurId2 == utilisateur_id)
                                        .Include(a => a.UtilisateurId1Navigation)
                                        .Include(a => a.UtilisateurId2Navigation)
                                        .ToList();

            if (listAmi == null || listAmi.Count == 0)
            {
                return NoContent();
            }

            // Création de la liste des utilisateurs amis
            List<Utilisateur> listUtilisateur = listAmi.Select(a =>
                a.UtilisateurId1 == utilisateur_id ? a.UtilisateurId2Navigation : a.UtilisateurId1Navigation
            ).ToList()!;

            if(listUtilisateur == null || !listUtilisateur.Any()) return NoContent();
            return Ok(listUtilisateur.ToArray());
        }
    }
}