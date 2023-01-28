using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using TriviaRoyale.Shared;

namespace TriviaRoyale.Client.Models
{
    public class DaddyService
    {
        private string roomID;
        public GameState GameState { get; set; } = GameState.Lobby;

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
        public Player PlayerAnswering { get; set; }
        public HubConnection hubConnection;
        public event Action OnChange;
        protected void NotifyStateChanged() => OnChange?.Invoke();

        public DaddyService(NavigationManager Navigation)
        {
            Console.WriteLine(Navigation.BaseUri + "Quiz");
            hubConnection = new HubConnectionBuilder()
                .WithUrl(Navigation.BaseUri + "Quiz")
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                })
                .WithAutomaticReconnect()
                .Build();

            hubConnection.On<Player[]>("NewPlayer", (players) =>
            {
                Console.WriteLine("connected players: " + players.Length);
                Players = players.ToList();
                NotifyStateChanged();
            });

            hubConnection.On<Player>("PlayerIsAnswering", (player) =>
            {
                Console.WriteLine($"Client Confirm: {player.Name} to answer");
                PlayerAnswering = player;
                NotifyStateChanged();
            });

            hubConnection.On<string>("ServerLog", Console.WriteLine);
            //hubConnection.On<string>("PlayerIsAnswering", (playerID) =>
            //{
            //	Console.WriteLine($"Player is answering: {playerID}");
            //	PlayerAnswering = Players.FirstOrDefault(p => p.ID == playerID);
            //	NotifyStateChanged();
            //});
            hubConnection.On("OpenQuestion", () =>
            {
                PlayerAnswering = null;
                NotifyStateChanged();
            });
            hubConnection.On<GameState>("StateChange", (state) =>
            {
                if(state == GameState.Playing)
                {
                    PlayerAnswering = null;
                }
                GameState = state;
                NotifyStateChanged();
            });

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
