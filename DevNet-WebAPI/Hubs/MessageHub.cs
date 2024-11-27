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
            lock (ConnectedUsers)
            {
                ConnectedUsers[userEmail] = Context.ConnectionId;
            }

            // Notificar al usuario actual que su conexión fue registrada
            await Clients.Caller.SendAsync("ConnectionRegistered", $"User {userEmail} is connected.");

            // Actualizar la lista de usuarios conectados en el frontend
            await Clients.All.SendAsync("UpdateUserList", ConnectedUsers.Keys.ToList());
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

        // Método que se ejecuta cuando un usuario se conecta
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        // Método que se ejecuta cuando un usuario se desconecta
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string? userEmailToRemove = null;

            // Buscar y eliminar el usuario desconectado
            lock (ConnectedUsers)
            {
                userEmailToRemove = ConnectedUsers
                    .FirstOrDefault(pair => pair.Value == Context.ConnectionId)
                    .Key;

                if (userEmailToRemove != null)
                {
                    ConnectedUsers.Remove(userEmailToRemove);
                }
            }

            // Notificar al frontend que la lista de usuarios ha cambiado
            if (userEmailToRemove != null)
            {
                await Clients.All.SendAsync("UpdateUserList", ConnectedUsers.Keys.ToList());
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
