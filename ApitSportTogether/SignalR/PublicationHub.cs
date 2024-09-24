using Microsoft.AspNetCore.SignalR;

namespace ApiSportTogether.SignalR
{
    public class PublicationHub : Hub
    {
        // Notification pour l'ajout d'un commentaire
        public async Task NotifyCommentAdded(int CommentaireId)
        {
            await Clients.Group(CommentaireId.ToString()).SendAsync("ReceiveCommentAdded", CommentaireId);
        }

        // Notification pour la suppression d'un commentaire
        public async Task NotifyCommentDeleted(int CommentaireId)
        {
            await Clients.Group(CommentaireId.ToString()).SendAsync("ReceiveCommentDeleted", CommentaireId);
        }

        // Notification pour la modification d'une publication
        public async Task NotifyPublicationUpdated(int publicationId)
        {
            await Clients.Group(publicationId.ToString()).SendAsync("ReceivePublicationUpdated", publicationId);
        }

        // Joindre un groupe spécifique basé sur l'ID de la publication
        public async Task JoinPublicationGroup(string publicationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, publicationId);
        }

        // Quitter un groupe spécifique
        public async Task LeavePublicationGroup(string publicationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, publicationId);
        }
    }


}
