using Microsoft.AspNetCore.SignalR;
using TriviaRoyale.Client.Models;
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
            await Clients.Groups(GetRoomName()).SendAsync("ReceiveAnswer", answer);
        }

        public async Task CreatePlayer(Player player)
        {
            var cookie = Context.GetHttpContext().Request.Cookies["playerId"];
            player.ID = cookie;
            Service.rooms.Find(x => x.Id == player.RoomID).AddPlayer(player);
            await Clients.Groups(player.RoomID).SendAsync("NewPlayer", Service.rooms.Find(x => x.Id == player.RoomID).Players.ToArray());
            await Clients.Caller.SendAsync("PlayerCreated", player);
        }


        public async Task WrongAnswer(Player player)
        {
            await Clients.Groups(GetRoomName()).SendAsync("StateChange", GameState.Playing);
        }
        public async Task CorrectAnswer(Player player)
        {
            var roomPlayer = Service.rooms.Find(x => x.Id == player.RoomID).Players.Find(x => x.ID == player.ID);
            roomPlayer.Points++;
            await Clients.Groups(GetRoomName()).SendAsync("StateChange", GameState.Playing);
            await Clients.Groups(GetRoomName()).SendAsync("NewPlayer", Service.rooms.Find(x => x.Id == player.RoomID).Players.ToArray());
        }

        public async Task PlayerClick(Player player)
        {
            await Clients.Groups(GetRoomName()).SendAsync("PlayerIsAnswering", player, GameState.PlayerToAnswer);
        }


        public async Task GetConnectedPlayers(string roomName)
        {
            await Clients.User(Context.UserIdentifier).SendAsync("NewPlayer", Service.rooms.Find(x => x.Id == roomName).Players.ToArray());

        }


        public async Task StartGame()
        {
            await Clients.Groups(GetRoomName()).SendAsync("StateChange", GameState.Playing);
        }

        public Task SendPrivateMessage(string user, string message)
        {
            return Clients.User(user).SendAsync("ReceiveMessage", message);
        }


        public async Task EndOfGame()
        {
            await Clients.Groups(GetRoomName()).SendAsync("StateChange", GameState.Ended);

        }
        public override Task OnConnectedAsync()
        {
            Console.WriteLine("User connected");

            return base.OnConnectedAsync();
        }

        public async Task JoinRoom(string roomName)
        {
            var room = Service.rooms.FirstOrDefault(x => x.Id == roomName);
            if(room.HostID == null)
            {
                room.HostID = Context.ConnectionId;
            }
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


        string GetRoomName()
        {
            var roomName = string.Empty;
            var room = Service.rooms.FirstOrDefault(x => x.HostID == Context.ConnectionId);
            if(room == null)
            {
                try
                {
                    room = Service.rooms.First(x => x.Players.Exists(y => y.ID == Context.ConnectionId));

                }
                catch(Exception)
                {

                    throw;
                }
            }
            return room.Id;
        }

    }
}
