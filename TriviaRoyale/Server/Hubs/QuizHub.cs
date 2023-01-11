using Microsoft.AspNetCore.SignalR;

namespace TriviaRoyale.Server.Hubs
{
    public class QuizHub : Hub
    {

        public async Task SendAnswer(string answer)
        {
            await Clients.All.SendAsync("ReceiveAnswer", answer);
        }

		public Task SendPrivateMessage(string user, string message)
		{
			return Clients.User(user).SendAsync("ReceiveMessage", message);
		}
		
		public override Task OnConnectedAsync()
        {
            Console.WriteLine("User connected");

            return base.OnConnectedAsync();
        }

		public async Task JoinRoom(string roomName)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

			await Clients.Group(roomName).SendAsync("ReceiveAnswer", $"{Context.ConnectionId} has joined the group {roomName}.");
		}

		public async Task LeaveRoom(string roomName)
		{
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);

			await Clients.Group(roomName).SendAsync("ReceiveAnswer", $"{Context.ConnectionId} has left the group {roomName}.");
		}

		public async Task SendMessageToRoom(string roomName, string message)
		{
			await Clients.Group(roomName).SendAsync("ReceiveAnswer", $"Message: {message}");
		}

		


    }
}
