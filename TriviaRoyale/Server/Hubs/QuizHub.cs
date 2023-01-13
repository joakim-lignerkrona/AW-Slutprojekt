using Microsoft.AspNetCore.SignalR;
using TriviaRoyale.Server.Models;
using TriviaRoyale.Shared;

namespace TriviaRoyale.Server.Hubs
{
    public class QuizHub : Hub
    {
        public RoomService Service { get; }

        public QuizHub(RoomService service)
        {
            Service = service;
        }
        public async Task SendAnswer(string answer)
        {
            await Clients.All.SendAsync("ReceiveAnswer", answer);
        }

        public async Task CreatePlayer(Player player)
        {
            player.SocketID = Context.ConnectionId;
            Service.rooms.Find(x => x.Id == player.RoomID).AddPlayer(player);
            await Groups.AddToGroupAsync(player.SocketID, player.RoomID);
            await Clients.Groups(player.RoomID).SendAsync("NewPlayer", Service.rooms.Find(x => x.Id == player.RoomID).Players.ToArray());


        }

        public async Task AnswerButton1()
        {
            await Clients.All.SendAsync("StateChange", GameState.PlayerToAnswer);

        }
        public async Task GetConnectedPlayers(string roomName)
        {
            await Clients.User(Context.UserIdentifier).SendAsync("NewPlayer", Service.rooms.Find(x => x.Id == roomName).Players.ToArray());

        }

        public async Task AnswerButton2()
        {
            await Clients.All.SendAsync("StateChange", GameState.OpponentToAnswer);

        }
        public async Task StartGame()
        {
            await Clients.All.SendAsync("StateChange", GameState.Playing);
        }

        public Task SendPrivateMessage(string user, string message)
        {
            return Clients.User(user).SendAsync("ReceiveMessage", message);
        }


        public async Task EndOfGame()
        {
            await Clients.All.SendAsync("StateChange", GameState.Ended);

        }
        public override Task OnConnectedAsync()
        {
            Console.WriteLine("User connected");

            return base.OnConnectedAsync();
        }

        public async Task JoinRoom(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync("ServerLog", $"{Context.ConnectionId} has joined the group {roomName}.");
        }


        public async Task LeaveRoom(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);

            await Clients.Group(roomName).SendAsync("ServerLog", $"{Context.ConnectionId} has left the group {roomName}.");
        }


        public async Task SendMessageToRoom(string roomName, string message)
        {
            await Clients.Group(roomName).SendAsync("ReceiveAnswer", $"Message: {message}");
        }




    }
}
