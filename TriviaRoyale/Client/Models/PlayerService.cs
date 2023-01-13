using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using TriviaRoyale.Shared;

namespace TriviaRoyale.Client.Models
{
    public class PlayerService : DaddyService
    {


        public Player Player { get; set; }

        public PlayerService(NavigationManager Navigation) : base(Navigation)
        {


            hubConnection.On<string>("ClickerName", (btn) =>
            {
                PlayerAnswering = btn;
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

