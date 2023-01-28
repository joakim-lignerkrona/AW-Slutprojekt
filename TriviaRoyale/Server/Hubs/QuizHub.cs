using Microsoft.AspNetCore.SignalR;
using System.Timers;
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
        public async Task NewGame()
        {
            Service.rooms.Find(r => r.Id == GetRoomName()).NewGame();
            await ChangeStateAsync(GetRoomName(), GameState.Lobby);
        }

        public async Task RegisterHost(string hostID, string roomID)
        {
            var room = Service.rooms.FirstOrDefault(r => r.Id == roomID);
            var cookie = Context.GetHttpContext().Request.Cookies["HostID"];
            if(room.HostID == null)
            {
                room.HostID = hostID;
                await Clients.Caller.SendAsync("HostAccepted", hostID);
                room.OnChange += () =>
                {

                    var timer = new System.Timers.Timer(100); // run every minute
                    timer.Elapsed += async (object sender, ElapsedEventArgs e) =>
                    {
                        await GetConnectedPlayers(room.Id);

                    };
                    timer.Start();
                    return;
                };

            }
            else if(room.HostID == cookie)
            {
                await Clients.Caller.SendAsync("HostAccepted", hostID);

            }
            await GetStateAsync(roomID);
        }
        public async Task CreatePlayer(Player player)
        {
            player.SocketID = Context.ConnectionId;
            var room = Service.rooms.Find(x => x.Id == player.RoomID);

            if(room.GameState == GameState.Lobby)
            {
                player.isActive = true;
            }
            else
            {
                player.isActive = false;
            }
            if(room.Players.FirstOrDefault(p => p.SocketID == player.SocketID) == null)
            {

                room.AddPlayer(player);
            }
            await Clients.Groups(player.RoomID).SendAsync("NewPlayer", Service.rooms.Find(x => x.Id == player.RoomID).Players.ToArray());
            await Clients.Caller.SendAsync("PlayerCreated", player);
        }
        public async Task RestorePlayer(string roomID)
        {
            var cookie = Context.GetHttpContext().Request.Cookies["playerId"];
            var player = Service.rooms.Find(x => x.Id == roomID).Players.Find(x => x.ID == cookie);
            if(player != null)
            {
                player.isActive = true;
                player.InactiveSince = null;
                player.SocketID = Context.ConnectionId;
                Console.WriteLine($"{player.Name} Reconnected");
                await Clients.Caller.SendAsync("PlayerCreated", player);
                await GetStateAsync(roomID);
            }
        }


        public async Task WrongAnswer(Player player)
        {
            var room = Service.rooms.Find(x => x.Id == player.RoomID);
            room.OpenQuestion();
            await ChangeStateAsync(GetRoomName(), GameState.Playing);

        }

        private async Task ChangeStateAsync(string RoomName, GameState state)
        {
            var room = Service.rooms.Find(x => x.Id == RoomName);
            room.GameState = state;
            await Clients.Groups(room.Id).SendAsync("StateChange", room.GameState);
            await LogToAll($"{RoomName} changed state to {state}");
        }
        public async Task GetStateAsync(string RoomName)
        {
            var room = Service.rooms.Find(x => x.Id == RoomName);
            await ChangeStateAsync(RoomName, room.GameState);
        }

        public async Task CorrectAnswer(Player player)
        {
            var room = Service.rooms.Find(x => x.Id == player.RoomID);
            room.OpenQuestion();
            var roomPlayer = room.Players.Find(x => x.ID == player.ID);
            roomPlayer.Points++;
            await Clients.Groups(GetRoomName()).SendAsync("NewPlayer", Service.rooms.Find(x => x.Id == player.RoomID).Players.ToArray());
            await ChangeStateAsync(GetRoomName(), GameState.Playing);
        }


        //Host ser resultatet:

        // 1. End game -> välj elimination round eller visa resultat.
        // 2. om eliminiation round: 
        // 2.1 Ny fråga från host: Om man ´svarar rätt -> välj att få +5 poäng eller reset all player points.

        //Att göra: två knappar kopplade till två olika funktioner. alt. host frågar vilket val som ska göras.

        //public async Task WrongHardAnswer(Player player)
        //{
        //    var roomPlayer = Service.rooms.Find(x => x.Id == player.RoomID).Players.Find(x => x.ID == player.ID);
        //    roomPlayer.Points -= 3;
        //    await Clients.Groups(GetRoomName()).SendAsync("StateChange", GameState.Playing);
        //}

        //public async Task CorrectHardAnswer(Player player) // Få 5 poäng
        //{
        //    var roomPlayer = Service.rooms.Find(x => x.Id == player.RoomID).Players.Find(x => x.ID == player.ID);
        //    roomPlayer.Points += 5;
        //    await Clients.Groups(GetRoomName()).SendAsync("StateChange", GameState.Playing);
        //    await Clients.Groups(GetRoomName()).SendAsync("NewPlayer", Service.rooms.Find(x => x.Id == player.RoomID).Players.ToArray());
        //}




        public async Task PlayerClick(Player player)
        {
            var room = Service.rooms.Find(r => r.Id == player.RoomID);

            lock(room)
            {
                room.PlayerDidAnswer(player);

            }

            await LogToAll($"{player.Name} want's to answer in {room.Id}, where {room.PlayerAnswering.Name} now has the word");
            await Clients.Groups(GetRoomName()).SendAsync("PlayerIsAnswering", player);
            await ChangeStateAsync(GetRoomName(), GameState.PlayerToAnswer);
        }


        public async Task GetConnectedPlayers(string roomName)
        {
            var room = Service.rooms.Find(x => x.Id == roomName);
            var players = room.Players;
            var parray = players.ToArray();
            await Clients.Caller.SendAsync("NewPlayer", parray);

        }


        public async Task StartGame()
        {
            await ChangeStateAsync(GetRoomName(), GameState.Playing);
        }

        public Task SendPrivateMessage(string user, string message)
        {
            return Clients.User(user).SendAsync("ReceiveMessage", message);
        }


        public async Task EndOfGame()
        {

            await ChangeStateAsync(GetRoomName(), GameState.Ended);

        }

        public async Task EndGameOrEliminationRound()
        {
            await Clients.Groups(GetRoomName()).SendAsync("StateChange", GameState.EndOrElimination);

        }



        public async Task EliminationRound() // Ny funktion -> gameState.Elimination/ HostDecision
        {
            await Clients.Groups(GetRoomName()).SendAsync("StateChange", GameState.EliminationRound);

        }
        public override Task OnConnectedAsync()
        {
            Console.WriteLine("User connected");

            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                Service.rooms.Find(x => x.Id == GetRoomName()).Players.ForEach(x =>
                {
                    if(x.SocketID == Context.ConnectionId)
                    {
                        x.isActive = false;
                        x.InactiveSince = DateTime.Now;
                    }
                });
            }
            catch(Exception)
            {
                Console.WriteLine("Player not in game left");
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task JoinRoom(string roomName)
        {
            var room = Service.rooms.FirstOrDefault(x => x.Id == roomName);

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
        public async Task LogToAll(string message)
        {
            Console.WriteLine(message);
            await Clients.All.SendAsync("ServerLog", $"Message: {message}");
        }

        string GetRoomName()
        {
            var cookie = Context.GetHttpContext().Request.Cookies["HostID"];
            var roomName = string.Empty;
            var room = Service.rooms.FirstOrDefault(x => x.HostID == cookie);
            if(room == null)
            {
                try
                {
                    room = Service.rooms.First(x => x.Players.Exists(y => y.SocketID == Context.ConnectionId));

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
