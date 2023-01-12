﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.NetworkInformation;
using TriviaRoyale.Shared;

namespace TriviaRoyale.Client.Models
{
    public class HostService
    {
        private string roomID;

        public string RoomID
        {
            get { return roomID; }
            set
            {
                roomID = value;
                NotifyStateChanged();
            }
        }


        public List<Player> Players { get; set; } = new();
        public Player playerAnswering { get; set; }
        public HubConnection? hubConnection;
        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public HostService(NavigationManager Navigation)
        {
            Console.WriteLine(Navigation.BaseUri + "Quiz");
            hubConnection = new HubConnectionBuilder()
                .WithUrl(Navigation.BaseUri + "Quiz")
                .Build();

            hubConnection.On<Player>("NewPlayer", (player) =>
            {
                Players.Add(player);
                NotifyStateChanged();
            });
            hubConnection.On<Player>("PlayerWantsToAnswer", (player) =>
                {
                    //TODO
                    //
                    //Läsa av svarstid (DateTime, Pinga?)
                    playerAnswering = player;
                    NotifyStateChanged();

                });



            hubConnection.On<string>("ServerLog", Console.WriteLine);
        }

        public async Task ConnectAsync()
        {
            await hubConnection.StartAsync();
            NotifyStateChanged();
        }
        public void StartGame()
        {
            hubConnection.SendAsync("StartGame");
        }

        public bool IsConnected =>
            hubConnection?.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }

    }
}
