using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using TriviaRoyale.Client.Shared.PlayerPage;
using TriviaRoyale.Shared;

namespace TriviaRoyale.Client.Models
{
    public class PlayerService : DaddyService
    {


        public Player Player { get; set; }
        public GameState GameState { get; set; } = GameState.Lobby;

        public PlayerService(NavigationManager Navigation) : base(Navigation)
        {

            hubConnection.On<Player>("PlayerCreated", (player) =>
            {
                Player = player;
            });

            hubConnection.On<string>("ClickerName", (btn) =>
            {
                PlayerToAnswer = btn;
                NotifyStateChanged();
            });

            hubConnection.On<string, GameState>("PlayerIsAnswering", (playerName, state) =>
            {
                GameState = state;
                NameOfClicker = playerName;
                NotifyStateChanged();
            });

            hubConnection.On<GameState>("StateChange", (state) =>
            {
                GameState = state;
                NotifyStateChanged();
            });
        }

    }

}

