using Microsoft.AspNetCore.SignalR;

namespace DevNet_WebAPI.Hubs
{
    public class MessageHub : Hub
    {
        // Diccionario para mapear usuarios y sus conexiones
        private static readonly Dictionary<string, string> ConnectedUsers = new();

        // Método para registrar la conexión de un usuario
        public async Task RegisterConnection(string userEmail)
        {
            ConnectedUsers[userEmail] = Context.ConnectionId;
            await Clients.Caller.SendAsync("ConnectionRegistered", $"User {userEmail} is connected.");
        }
        
        // Método para enviar un mensaje de un usuario a otro
        public async Task SendMessage(string senderEmail, string receiverEmail, string message)
        {
            if (ConnectedUsers.TryGetValue(receiverEmail, out var receiverConnectionId))
            {
                await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", senderEmail, message);
            }
            else
            {
                await Clients.Caller.SendAsync("UserOffline", $"User {receiverEmail} is offline.");
            }
        }
    }
}