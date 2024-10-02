using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.ObjectContext;
using ApiSportTogether.model.ObjectVue;
using ApiSportTogether.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ApiSportTogether.Controller
{
    [ApiController]
    [Route("ApiSportTogether/[controller]")]
    public class PublicationController : ControllerBase
    {
        private readonly SportTogetherContext _context;
        private readonly IHubContext<PublicationHub> _hubContext;
        public PublicationController(SportTogetherContext context, IHubContext<PublicationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
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
                .OrderByDescending(p => p.DatePublication)
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

        // PUT: ApiSportTogether/Publication/UpdatePublication/{id}
        [HttpPut("UpdatePublication/{id}")]
        public async Task<IActionResult> UpdatePublication(int id, [FromBody] Publication publication)
        {
            if (id != publication.PublicationsId)
            {
                return BadRequest();
            }

            _context.Entry(publication).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();

                // Notifier via SignalR que la publication a été modifiée
                await _hubContext.Clients.Group(publication.PublicationsId.ToString())
                    .SendAsync("ReceivePublicationUpdated", publication.PublicationsId);
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

        // DELETE: ApiSportTogether/Publication/DeletePublication/5
        [HttpDelete("DeletePublication/{id}")]
        public IActionResult DeletePublication(int id)
        {
            Publication publication = _context.Publications.Include(p => p.EncouragementPublications).Include(p => p.PublicationImages).Include(p => p.PublicationCommentaires).ThenInclude(pc => pc.EncouragementPublicationCommentaires).Where(p => p.PublicationsId == id).FirstOrDefault()!;
            if (publication == null)
            {
                return NotFound();
            }
            else
            {
                _context.PublicationImages.RemoveRange(publication.PublicationImages);
                _context.SaveChanges();
                
                if (publication.PublicationCommentaires != null && publication.PublicationCommentaires.Any())
                {
                    foreach(PublicationCommentaire publicationCommentaire in publication.PublicationCommentaires)
                    {
                        _context.EncouragementPublicationCommentaires.RemoveRange(publicationCommentaire.EncouragementPublicationCommentaires);
                        _context.SaveChanges();
                    }
                    _context.PublicationCommentaires.RemoveRange(publication.PublicationCommentaires);
                    _context.SaveChanges();
                }

                
               

                if (publication.EncouragementPublications != null && publication.EncouragementPublications.Any())
                {
                    _context.EncouragementPublications.RemoveRange(publication.EncouragementPublications);
                    _context.SaveChanges();
                }
              
            }

            _context.Publications.Remove(publication);
            _context.SaveChanges();

            return NoContent();
        }
        // GET: Publication/GetPublicationVueById/1
        [HttpGet("GetPublicationVueById/{publicationId}/{utilisateurId}")]
        public ActionResult<PublicationVue> GetPublicationVueById(int publicationId, int utilisateurId)
        {
            var publications = _context.Publications
                .Where(p => p.PublicationsId == publicationId)
                .Include(p => p.Utilisateur).ThenInclude(u => u.ProfileImages)
                .Include(p => p.PublicationImages)
                .Include(p => p.PublicationCommentaires)
                .Include(p => p.EncouragementPublications)
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
                    tempsDiff = GetDateDifference(p.DatePublication, DateTime.Now),
                    IsEncourager = p.EncouragementPublications.Any(ep => ep.UtilisateurId == utilisateurId && ep.PublicationId == p.PublicationsId),
                    SportTag = p.SportTag,
                    Visibilite = p.Visibilite,

                })
                .FirstOrDefault();

            if (publications == null)
            {
                return NotFound("Aucune publication trouvée pour cet utilisateur.");
            }
            publications.Commentaires = _context.PublicationCommentaires.Where(pc => pc.PublicationId == publicationId).Include(pc => pc.EncouragementPublicationCommentaires).Select(c => new CommentaireVue
            {
                CommentaireId = c.CommentaireId,
                UtilisateurId = c.UtilisateurId,
                PseudoUtilisateur = c.Utilisateur.Pseudo, // Remplacer par le champ du pseudo
                Contenu = c.Contenu,
                DateCommentaire = c.DateCommentaire,
                NombreEncouragementCommentaire = c.NombreEncouragementCommentaire,
                ImageUtilisateurUrl = _context.ProfileImages.Where(pi => pi.UtilisateursId == c.UtilisateurId).FirstOrDefault()!.Url, // URL de l'image de profil de l'utilisateur du commentaire
                IsEncouragerCom = c.EncouragementPublicationCommentaires.Any(epc => epc.UtilisateurId == utilisateurId && epc.PublicationCommentaireId == c.CommentaireId)
            }).ToArray();
            if (publications.Commentaires != null)
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

        // GET: Publication/GetPublicationsIdsPourFilActualite/1
        [HttpGet("GetPublicationsIdsPourFilActualite/{utilisateurId}")]
        public ActionResult<IEnumerable<int>> GetPublicationsIdsPourFilActualite(int utilisateurId)
        {
            Utilisateur utilisateur = _context.Utilisateurs.Where(u => u.UtilisateursId == utilisateurId).Include(u => u.SportFavoris).ThenInclude(sf => sf.Sports).Include(u => u.Publications).FirstOrDefault()!;
            List<Publication> listPublication = new();
            List<int> listClePublication = new();

            if (utilisateur == null) return NotFound("Il n'y a pas d'utilisateur avec cette id");
            if(utilisateur.Publications != null)
            {
                listPublication.AddRange(utilisateur.Publications);
            }


            // Récupérer la liste des relations d'amitié impliquant l'utilisateur
            List<Ami> listAmi = _context.Amis
                                        .Where(a => a.UtilisateurId1 == utilisateurId || a.UtilisateurId2 == utilisateurId)
                                        .Include(a => a.UtilisateurId1Navigation)
                                        .Include(a => a.UtilisateurId2Navigation)
                                        .ToList();
            // Création de la liste des utilisateurs amis
            List<Utilisateur> listUtilisateur = listAmi.Select(a =>
                a.UtilisateurId1 == utilisateurId ? a.UtilisateurId2Navigation : a.UtilisateurId1Navigation
            ).ToList()!;

            if (listUtilisateur != null && listUtilisateur.Any())
            {
                foreach (Utilisateur u in listUtilisateur)
                {
                    var publicationIds = _context.Publications.Where(p => p.UtilisateurId == u.UtilisateursId)
                .OrderByDescending(p => p.DatePublication)
                .ToList();
                    if (publicationIds != null   && publicationIds.Any())
                    {
                        listPublication.AddRange(publicationIds);
                    }
                    
                }
            }
            List<string>? sportFavori = null;
            if (utilisateur.SportFavoris != null)
                sportFavori = utilisateur.SportFavoris.Select(sf => sf.Sports.Nom).ToList();

            if(sportFavori != null)
            {
                foreach(string sf in sportFavori)
                {
                    List<Publication>? listPublicationParSport = _context.Publications.Where(p => p.SportTag == sf && p.UtilisateurId != utilisateurId && p.Visibilite == true).ToList();
                    if(listPublicationParSport != null )
                    listPublication.AddRange(listPublicationParSport);
                }
            }
            if (!listPublication.Any())
            {
                return NotFound("Aucune publication trouvée pour ce fil d'actualité");
            }
            listClePublication = listPublication.OrderByDescending(p => p.DatePublication).Select(p => p.PublicationsId).ToList();
            return Ok(listClePublication.ToArray());
        }


        public static string GetDateDifference(DateTime startDate, DateTime endDate)
        {
            // S'assurer que la date de fin est postérieure à la date de début
            if (endDate < startDate)
            {
                throw new ArgumentException("La date de fin doit être postérieure à la date de début.");
            }

            // Calculer la différence entre les dates
            var totalDays = (endDate - startDate).Days;
            var totalHours = (endDate - startDate).Hours;
            var totalMinutes = (endDate - startDate).Minutes;

            // Conversion des unités
            int years = 0, months = 0, days = 0, hours = 0, minutes = 0;

            // On part des jours totaux pour calculer le nombre d'années
            while (totalDays >= 365)
            {
                years++;
                totalDays -= 365; // On suppose une année de 365 jours
            }

            // On part des jours restants pour calculer le nombre de mois
            while (totalDays >= 30)
            {
                months++;
                totalDays -= 30; // On suppose un mois de 30 jours
            }

            // Les jours restants après le calcul des mois
            days = totalDays;

            // Ajout des heures totales
            hours += totalHours;

            // Ajustement des minutes
            minutes += totalMinutes;

            // Ajustement si les minutes dépassent 60
            if (minutes >= 60)
            {
                hours += minutes / 60;
                minutes %= 60; // Reste des minutes
            }

            // Ajustement si les heures dépassent 24
            if (hours >= 24)
            {
                days += hours / 24;
                hours %= 24; // Reste des heures
            }

            // Ajustement si les jours dépassent 31
            if (days >= 30)
            {
                months += days / 30; // Conversion en mois
                days %= 30; // Reste des jours
            }

            // Retourne la plus grande unité de temps
            if (years > 0)
            {
                return $"{years} année{(years > 1 ? "s" : "")}";
            }
            else if (months > 0)
            {
                return $"{months} mois";
            }
            else if (days > 0)
            {
                return $"{days} jour{(days > 1 ? "s" : "")}";
            }
            else if (hours > 0)
            {
                return $"{hours} heure{(hours > 1 ? "s" : "")}";
            }
            else
            {
                return $"{minutes} minute{(minutes > 1 ? "s" : "")}";
            }
        }
    }
}
