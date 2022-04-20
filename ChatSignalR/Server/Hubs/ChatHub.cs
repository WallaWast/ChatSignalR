using Microsoft.AspNetCore.SignalR;

namespace ChatSignalR.Server.Hubs
{
    public class ChatHub : Hub
    {
        private static Dictionary<string, string> Users = new();

        public override async Task OnConnectedAsync()
        {
            string username = Context.GetHttpContext().Request.Query["username"];
            Users.Add(Context.ConnectionId, username);
            await AddMessageToChat(string.Empty, $"{username} joined!");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            string userName = Users[Context.ConnectionId];
            Users.Remove(Context.ConnectionId);
            await AddMessageToChat(string.Empty, $"{userName} left!");
        }

        public async Task AddMessageToChat(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message, Users.Count);
        }
    }
}
