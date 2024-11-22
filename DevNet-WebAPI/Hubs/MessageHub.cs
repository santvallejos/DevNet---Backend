using Microsoft.AspNetCore.SignalR;

namespace DevNet_WebAPI.Hubs
{
    public class MessageHub : Hub
    {
        // Diccionario para mapear usuarios y sus conexiones
        private static readonly Dictionary<string, string> ConnectedUsers = new();

        // Método para registrar la conexión de un usuario
        public async Task RegisterConnection(string userId)
        {
            ConnectedUsers[userId] = Context.ConnectionId;
            await Clients.Caller.SendAsync("ConnectionRegistered", $"User {userId} is connected.");
        }
        
        // Método para enviar un mensaje de un usuario a otro
        public async Task SendMessage(string senderId, string receiverId, string message)
        {
            if (ConnectedUsers.TryGetValue(receiverId, out var receiverConnectionId))
            {
                await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", senderId, message);
            }
            else
            {
                await Clients.Caller.SendAsync("UserOffline", $"User {receiverId} is offline.");
            }
        }
    }
}
