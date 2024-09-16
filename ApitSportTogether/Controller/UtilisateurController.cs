using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.ObjectContext;
using ApiSportTogether.model.ObjectVue;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ApiSportTogether.Controller
{
    [ApiController]
    [Route("ApiSportTogether/[controller]")]
    public class UtilisateurController : ControllerBase
    {
        private readonly SportTogetherContext _context;
        private readonly IConfiguration _configuration;

        public UtilisateurController(IConfiguration configuration, SportTogetherContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: ApiSportTogether/Utilisateur
        [HttpGet]
        public ActionResult<List<Utilisateur>> GetUtilisateurs()
        {
            return _context.Utilisateurs
                           .Include(u => u.ProfileImages)
                           .ToList();
        }

        // GET: ApiSportTogether/Utilisateur/5
        [HttpGet("{id}")]
        public ActionResult<Utilisateur> GetUtilisateurById(int id)
        {
            var utilisateur = _context.Utilisateurs
                                       .Include(u => u.ProfileImages)
                                       .FirstOrDefault(u => u.UtilisateursId == id);

            return utilisateur == null ? NotFound() : utilisateur;
        }

        // POST: ApiSportTogether/Utilisateur/CreateUtilisateur
        [HttpPost("CreateUtilisateur")]
        public ActionResult<Utilisateur> PostUtilisateur([FromBody]Utilisateur utilisateur)
        {
            if (utilisateur != null)
            {
                utilisateur.MotDePasse = HashPassword(utilisateur.MotDePasse);
                _context.Utilisateurs.Add(utilisateur);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetUtilisateurById), new { id = utilisateur.UtilisateursId }, utilisateur);
            }
            else
            { return NotFound(); }

        }

        // PUT: ApiSportTogether/Utilisateur/5
        [HttpPut("{id}")]
        public ActionResult PutUtilisateur(int id, Utilisateur utilisateur)
        {
            if (id != utilisateur.UtilisateursId)
            {
                return BadRequest();
            }

            _context.Entry(utilisateur).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Utilisateurs.Any(u => u.UtilisateursId == id))
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
        // PUT: ApiSportTogether/Utilisateur/ModifierMotDePasse/1
        [HttpPut("ModifierMotDePasse/{id}")]
        public ActionResult ModifierMotDePasse(int id, [FromBody] string password)
        {
            Utilisateur utilisateur = _context.Utilisateurs.Find(id);
            if (utilisateur == null)
            {
                return NoContent();
            }
            utilisateur.MotDePasse = HashPassword(password);
            _context.Entry(utilisateur).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Utilisateurs.Any(u => u.UtilisateursId == id))
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

        private string HashPassword(string password)
        {
            var hasher = new PasswordHasher<object>();
            return hasher.HashPassword(null, password);
        }

        // DELETE: ApiSportTogether/Utilisateur/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUtilisateur(int id)
        {
            var utilisateur = _context.Utilisateurs.Find(id);
            if (utilisateur == null)
            {
                return NotFound();
            }

            _context.Utilisateurs.Remove(utilisateur);
            _context.SaveChanges();

            return NoContent();
        }

        // GET: ApiSportTogether/UtilisateurVueById/5
        [HttpGet("UtilisateurVueById/{id}")]
        public ActionResult<UtilisateurVue> GetUtilisateurVueById(int id)
        {
            // Récupérer l'utilisateur depuis la base de données (exemple avec Entity Framework)
            var utilisateur = _context.Utilisateurs.FirstOrDefault(u => u.UtilisateursId == id);

            if (utilisateur == null)
            {
                return NotFound();
            }

            // Exemple : Récupérer les données supplémentaires (à ajuster selon ton modèle et tes calculs)
            int nombreAuteurAnnonce = _context.Annonces.Count(a => a.Auteur == id);
            int nombreAnnonceEffectuer = _context.Participations.Count(p => p.UtilisateurId == id);
            List<Annonce>? listAnnonce = _context.Annonces.Where(a => a.Auteur == id).ToList();
            List<decimal?> listNoteAnnonce = new();
            decimal? noteMoyenneDesAnnonces = null;
            if (listAnnonce != null)
            {
                if (listAnnonce.Any())
                {
                    foreach (var annonce in listAnnonce)
                    {
                        listNoteAnnonce.Add(annonce.NoteAnnonce);
                    }
                    decimal noteTotale = 0;
                     foreach (var a in listNoteAnnonce)
                    {
                        noteTotale = (decimal)(noteTotale + a)!;  
                    }
                     if(noteTotale > 0)
                    {
                        noteMoyenneDesAnnonces = noteTotale / listAnnonce.Count();
                    }
                  
                }
            }
          
                var premiereParticipationOuAnnonce = _context.Participations
        .Where(p => p.UtilisateurId == id)
        .OrderBy(p => p.DateParticipation)
        .Select(p => p.DateParticipation)
        .FirstOrDefault();  // Récupérer la première date de participation

            if (premiereParticipationOuAnnonce == DateTime.MinValue)
            {
                // Si aucune participation trouvée, on prend la date courante pour éviter une division par zéro
                premiereParticipationOuAnnonce = DateTime.Now;
            }

            decimal moisDepasses = ((DateTime.Now - premiereParticipationOuAnnonce).Value.Days / 30.0m);
            if (moisDepasses <= 0)
            {
                moisDepasses = 1;  // Pour éviter une division par 0, on force au moins 1 mois
            }

            decimal? annonceEffectuerParMoisMoyenne = nombreAnnonceEffectuer / moisDepasses;



         
            // Exemple : Récupérer les sports favoris (adapter selon tes relations)
            List<string> topTroisSport = _context.SportFavoris
                                        .Where(s => s.UtilisateursId == id)
                                        .Take(3)
                                        .Include(sf => sf.Sports)
                                        .Select(s => s.Sports!.Nom)
                                        .ToList()!;
            string urlProfilImage = _context.ProfileImages.Where(pi => pi.UtilisateursId == id).FirstOrDefault()!.Url!;
            // Convertir en UtilisateurVue
            var utilisateurVue = ConvertirEnUtilisateurVue(utilisateur, nombreAuteurAnnonce, nombreAnnonceEffectuer, noteMoyenneDesAnnonces ?? 0, annonceEffectuerParMoisMoyenne ?? 0, topTroisSport, urlProfilImage);

            // Retourner l'objet
            return Ok(utilisateurVue);
        }
        private  UtilisateurVue ConvertirEnUtilisateurVue(Utilisateur utilisateur, int nombreAuteurAnnonce, int nombreAnnonceEffectuer, decimal noteMoyenneDesAnnonces, decimal annonceEffectuerParMoisMoyenne, List<string> topTroisSport, string url)
        {
            return new UtilisateurVue
            {
                UtilisateursId = utilisateur.UtilisateursId,
                Nom = utilisateur.Nom,
                Prenom = utilisateur.Prenom,
                Pseudo = utilisateur.Pseudo,
                Genre = utilisateur.Genre,
                Age = utilisateur.Age,
                Ville = utilisateur.Ville,
                Description = utilisateur.Description,
                NombreAuteurAnnonce = nombreAuteurAnnonce,
                NombreAnnonceEffectuer = nombreAnnonceEffectuer,
                NoteMoyenneDesAnnonces = noteMoyenneDesAnnonces,
                AnnonceEffectuerParMoisMoyenne = annonceEffectuerParMoisMoyenne,
                TopTroisSport = topTroisSport.ToArray(),
                urlProfilImage = url
            };
        }

    }
}
