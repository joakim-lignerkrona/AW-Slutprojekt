using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using TriviaRoyale.Shared;

namespace TriviaRoyale.Client.Models
{
    public class DaddyService
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
        public HubConnection? hubConnection;
        public event Action OnChange;
        protected void NotifyStateChanged() => OnChange?.Invoke();

        public DaddyService(NavigationManager Navigation)
        {
            Console.WriteLine(Navigation.BaseUri + "Quiz");
            hubConnection = new HubConnectionBuilder()
                .WithUrl(Navigation.BaseUri + "Quiz")
                .Build();

            hubConnection.On<Player[]>("NewPlayer", (players) =>
            {
                Console.WriteLine("connected players: " + players.Length);
                Players = players.ToList();
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
        public void GetPlayers()
        {
            hubConnection.InvokeAsync("GetConnectedPlayers", RoomID);
        }

        public bool IsConnected =>
            hubConnection?.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if(hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }
    }
}
