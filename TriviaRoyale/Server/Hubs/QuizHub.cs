﻿using Microsoft.AspNetCore.SignalR;
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
        public async Task RegisterHost(string hostID, string roomID)
        {
            var room = Service.rooms.FirstOrDefault(r => r.Id == roomID);
            var cookie = Context.GetHttpContext().Request.Cookies["HostID"];
            if(room.HostID == null)
            {
                room.HostID = hostID;
                await Clients.Caller.SendAsync("HostAccepted", hostID);

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

            room.AddPlayer(player);
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
                await Clients.Caller.SendAsync("PlayerCreated", player);
                await GetStateAsync(roomID);
            }
        }


        public async Task WrongAnswer(Player player)
        {
            await ChangeStateAsync(GetRoomName(), GameState.Playing);

        }

        private async Task ChangeStateAsync(string RoomName, GameState state)
        {
            var room = Service.rooms.Find(x => x.Id == RoomName);
            room.GameState = state;
            await Clients.Groups(room.Id).SendAsync("StateChange", room.GameState);
        }
        public async Task GetStateAsync(string RoomName)
        {
            var room = Service.rooms.Find(x => x.Id == RoomName);
            await ChangeStateAsync(RoomName, room.GameState);
        }

        public async Task CorrectAnswer(Player player)
        {
            var roomPlayer = Service.rooms.Find(x => x.Id == player.RoomID).Players.Find(x => x.ID == player.ID);
            roomPlayer.Points++;
            await Clients.Groups(GetRoomName()).SendAsync("NewPlayer", Service.rooms.Find(x => x.Id == player.RoomID).Players.ToArray());
            await ChangeStateAsync(GetRoomName(), GameState.Playing);
        }

        public async Task PlayerClick(Player player)
        {
            await Clients.Groups(GetRoomName()).SendAsync("PlayerIsAnswering", player);
            await ChangeStateAsync(GetRoomName(), GameState.PlayerToAnswer);
        }


        public async Task GetConnectedPlayers(string roomName)
        {
            await Clients.Caller.SendAsync("NewPlayer", Service.rooms.Find(x => x.Id == roomName).Players.ToArray());

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
        public override Task OnConnectedAsync()
        {
            Console.WriteLine("User connected");

            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            Service.rooms.Find(x => x.Id == GetRoomName()).Players.ForEach(x =>
            {
                if(x.SocketID == Context.ConnectionId)
                {
                    x.isActive = false;
                    x.InactiveSince = DateTime.Now;
                }
            });
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
