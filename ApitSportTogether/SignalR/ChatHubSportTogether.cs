using Microsoft.AspNetCore.SignalR;

namespace ApiSportTogether.SignalR
{
    public class ChatHubSportTogether : Hub
    {
        // Envoyer un message à tous les membres d'un groupe
        public async Task SendMessageToGroup(string groupName, string user, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
        }

        // Rejoindre un groupe spécifique
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        // Quitter un groupe spécifique
        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
