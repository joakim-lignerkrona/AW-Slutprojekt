using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using TriviaRoyale.Shared;

namespace TriviaRoyale.Client.Models
{
    public class PlayerService
    {

        public string NameOfClicker { get; set; }
        public Player Player { get; set; }
        public string RoomID { get; set; }
        public GameState GameState { get; set; } = GameState.Lobby;
        public List<Player> Players { get; set; } = new();
        public HubConnection? hubConnection;
        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public PlayerService(NavigationManager Navigation)
        {
            Console.WriteLine(Navigation.BaseUri + "Quiz");
            hubConnection = new HubConnectionBuilder()
                .WithUrl(Navigation.BaseUri + "Quiz")
                .Build();

            hubConnection.On<string>("ClickerName", (btn) =>
            {
                NameOfClicker = btn;
                NotifyStateChanged();
            });

            hubConnection.On<string, GameState>("PlayerIsAnswering", (playerName, state) =>
            {
                GameState = state;
                NameOfClicker = playerName;
                NotifyStateChanged();
            });



            hubConnection.On<Player>("NewPlayer", (player) =>
            {
                Players.Add(player);
                NotifyStateChanged();
            });

            hubConnection.On<GameState>("StateChange", (state) =>
            {
                GameState = state;
                NotifyStateChanged();
            });
        }

        public async Task ConnectAsync()
        {
            await hubConnection.StartAsync();
            NotifyStateChanged();
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

