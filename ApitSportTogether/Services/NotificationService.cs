using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.ObjectContext;
using Microsoft.EntityFrameworkCore;

namespace ApiSportTogether.Services
{
    public class NotificationService
    {
        private readonly SportTogetherContext _context;

        public NotificationService(SportTogetherContext context)
        {
            _context = context;
        }

        // Méthode pour envoyer des notifications aux participants
        public async Task NotifierParticipantsAsync()
        {
            // Sélectionner les participations des annonces publiées il y a plus de 1 jour
            var participations = await _context.Participations
                .Include(p => p.Annonce)
                .Where(p => p.Annonce.DateHeureAnnonce <= DateTime.Now.AddDays(-1))
                .ToListAsync();

            foreach (var participation in participations)
            {
                // Créer le contenu de la notification
                var contenu = $"Veuillez évaluer l'annonce {participation.Annonce.Titre}";

                // Créer la notification
                var notification = new NotificationUtilisateur
                {
                    UtilisateurId = (int)participation.UtilisateurId,
                    TypeNotification = "Note",
                    DateNotification = DateTime.Now,
                    Vu = false,
                    Contenu = contenu
                };

                // Ajouter la notification dans la base de données
                _context.NotificationUtilisateurs.Add(notification);
            }

            // Sauvegarder les modifications dans la base de données
            await _context.SaveChangesAsync();
        }
    }

}
