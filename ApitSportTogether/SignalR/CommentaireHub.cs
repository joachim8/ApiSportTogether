using Microsoft.AspNetCore.SignalR;

namespace ApiSportTogether.SignalR
{
    public class CommentaireHub : Hub
    {
        // Notification pour l'ajout EncouragementCommentaire
        public async Task NotifyEncouragementCommentaireAdded(int commentaireId, string nbreEncouragement)
        {
            await Clients.Group(commentaireId.ToString()).SendAsync("ReceivePublicationAdded", commentaireId, nbreEncouragement);
        }
        // Notification pour la suppression EncouragementCommentaire
        public async Task NotifyEncouragementCommentaireDeleted(int commentaireId, string nbreEncouragement)
        {
            await Clients.Group(commentaireId.ToString()).SendAsync("ReceivePublicationDeleted", commentaireId, nbreEncouragement);
        }

        #region "Group commentaire"
        // Joindre un groupe spécifique basé sur l'ID de la publication
        public async Task JoinPublicationGroup(string commentaireId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, commentaireId);
        }

        // Quitter un groupe spécifique
        public async Task LeavePublicationGroup(string commentaireId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, commentaireId);
        }
        #endregion
    }
}
