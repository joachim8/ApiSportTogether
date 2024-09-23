using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.ObjectContext;
using ApiSportTogether.model.ObjectVue;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiSportTogether.Controller
{
    [ApiController]
    [Route("ApiSportTogether/[controller]")]
    public class PublicationController : ControllerBase
    {
        private readonly SportTogetherContext _context;

        public PublicationController(SportTogetherContext context)
        {
            _context = context;
        }

        // GET: ApiSportTogether/Publication
        [HttpGet]
        public ActionResult<List<Publication>> GetPublications()
        {
            return _context.Publications
                           .Include(p => p.PublicationImages)
                           .ToList();
        }
        // GET: Publications/GetPublicationsIdsByUtilisateur/1
        [HttpGet("GetPublicationsIdsByUtilisateur/{utilisateurId}")]
        public ActionResult<IEnumerable<int>> GetPublicationsIdsByUtilisateur(int utilisateurId)
        {
            var publicationIds = _context.Publications
                .Where(p => p.UtilisateurId == utilisateurId)
                .Select(p => p.PublicationsId)
                .ToList();

            if (publicationIds == null || publicationIds.Count == 0)
            {
                return NotFound("Aucune publication trouvée pour cet utilisateur.");
            }

            return Ok(publicationIds.ToArray());
        }
        // GET: Publications/GetPublicationsIdsByUtilisateur/1
        [HttpGet("GetPublicationById/{publicationId}")]
        public ActionResult<Publication> GetPublicationById(int publicationId)
        {
            var publication = _context.Publications.Find(publicationId);

            if (publication == null)
            {
                return NotFound("Publication non trouvée.");
            }

            return Ok(publication);
        }


        // POST: ApiSportTogether/Publication/CreatePublication
        [HttpPost("CreatePublication")]
        public ActionResult<Publication> PostPublication([FromBody] Publication publication)
        {
            if (publication == null)
            {
                return BadRequest();
            }

            _context.Publications.Add(publication);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetPublicationById), new { id = publication.PublicationsId }, publication);
        }

        // PUT: ApiSportTogether/Publication/5
        [HttpPut("{id}")]
        public ActionResult PutPublication(int id, Publication publication)
        {
            if (id != publication.PublicationsId)
            {
                return BadRequest();
            }

            _context.Entry(publication).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Publications.Any(p => p.PublicationsId == id))
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

        // DELETE: ApiSportTogether/Publication/5
        [HttpDelete("{id}")]
        public IActionResult DeletePublication(int id)
        {
            var publication = _context.Publications.Find(id);
            if (publication == null)
            {
                return NotFound();
            }

            _context.Publications.Remove(publication);
            _context.SaveChanges();

            return NoContent();
        }
        // GET: Publication/GetPublicationVueById/1
        [HttpGet("GetPublicationVueById/{publicationId}")]
        public ActionResult<PublicationVue> GetPublicationVueById(int publicationId)
        {
            var publications = _context.Publications
                .Where(p => p.PublicationsId == publicationId)
                .Include(p => p.Utilisateur).ThenInclude(u => u.ProfileImages)
                .Include(p => p.PublicationImages)
                .Include(p => p.PublicationCommentaires)
                .Select(p => new PublicationVue
                {
                    PublicationsId = p.PublicationsId,
                    UtilisateurId = p.UtilisateurId,
                    Contenu = p.Contenu,
                    DatePublication = p.DatePublication,
                    PseudoUtilisateur = p.Utilisateur.Pseudo, // Remplacer par le bon champ du pseudo utilisateur
                    ImageUtilisateurUrl = p.Utilisateur.ProfileImages.FirstOrDefault()!.Url, // Remplacer par l'URL de l'image de profil
                    MediaUrls = p.PublicationImages.Select(i => i.Url).ToArray()!, // URLs des médias (images/vidéos)
                    NombreEncouragements = p.NombreEncouragement,
                })
                .FirstOrDefault();

            if (publications == null)
            {
                return NotFound("Aucune publication trouvée pour cet utilisateur.");
            }
            publications.Commentaires = _context.PublicationCommentaires.Where(pc => pc.PublicationId == publicationId).Select(c => new CommentaireVue
                {
                    CommentaireId = c.CommentaireId,
                    UtilisateurId = c.UtilisateurId,
                    PseudoUtilisateur = c.Utilisateur.Pseudo, // Remplacer par le champ du pseudo
                    Contenu = c.Contenu,
                    DateCommentaire = c.DateCommentaire,
                    NombreEncouragementCommentaire = c.NombreEncouragementCommentaire,
                    ImageUtilisateurUrl = _context.ProfileImages.Where(pi => pi.UtilisateursId == c.UtilisateurId).FirstOrDefault()!.Url // URL de l'image de profil de l'utilisateur du commentaire
                }).ToArray();
            if (publications.Commentaires == null)
            {
                
            }

            return Ok(publications);
        }

        //Post : ApiSportTogether/Publication/AjouterPublicationAvecMedias
        [HttpPost("AjouterPublicationAvecMedias")]
        public async Task<ActionResult> AjouterPublicationAvecMedias([FromBody] PublicationAjoutVue request)
        {
            if (request.Publication == null)
            {
                return BadRequest("Données invalides.");
            }
            if (request.Publication.NombreEncouragement == null) request.Publication.NombreEncouragement = 0;
            // Ajout de la nouvelle publication dans la base de données
            _context.Publications.Add(request.Publication);
            _context.SaveChanges(); // Sauvegarder la publication pour obtenir l'ID

            // Ajout des médias associés (si présents)
            if (request.MediaUrls != null && request.MediaUrls.Any())
            {
                foreach (var mediaUrl in request.MediaUrls)
                {
                    // Vérifier si l'URL correspond à une image ou une vidéo en fonction de l'extension
                    var extension = Path.GetExtension(mediaUrl).ToLower();
                    var mediaType = (extension == ".jpg" || extension == ".png" || extension == ".jpeg") ? "image" : "video";

                    // Enregistrer les informations du média dans la base de données
                    var publicationMedia = new PublicationImage
                    {
                        Url = mediaUrl,  // URL provenant de l'API d'upload
                        PublicationsId = request.Publication.PublicationsId,
                        Timestamp = DateTime.Now,
                        Type = mediaType  // Défini à "image" ou "video"
                    };

                    _context.PublicationImages.Add(publicationMedia);
                }

                await _context.SaveChangesAsync();  // Sauvegarder les médias associés dans la base de données
            }

            return Ok(request);
        }
    }
}
