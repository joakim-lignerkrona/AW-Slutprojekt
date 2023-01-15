using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using TriviaRoyale.Shared;

namespace TriviaRoyale.Client.Models
{
    public class PlayerService : DaddyService
    {

        public string NameOfClicker { get; set; }
        public Player Player { get; set; }

        public PlayerService(NavigationManager Navigation) : base(Navigation)
        {


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


        }

    }

}

