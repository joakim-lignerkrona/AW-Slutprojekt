using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using TriviaRoyale.Shared;

namespace TriviaRoyale.Client.Models
{
    public class PlayerService : DaddyService
    {
        public Player Player { get; set; }
        public GameState GameState { get; set; } = GameState.Lobby;

        public PlayerService(NavigationManager Navigation) : base(Navigation)
        {

            hubConnection.On<GameState>("StateChange", (state) =>
            {
                GameState = state;
                NotifyStateChanged();
            });
        }
    }

}

