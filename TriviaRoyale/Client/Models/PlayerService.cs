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

            hubConnection.On<Player>("PlayerCreated", (player) =>
            {
                Player = player;
                NotifyStateChanged();
            });

            hubConnection.On<Player[]>("NewPlayer", (players) =>
            {
                Player = players.FirstOrDefault(p => p.ID == Player.ID);
                NotifyStateChanged();
            });

            hubConnection.On<Player>("PlayerIsAnswering", (player) =>
            {
                PlayerAnswering = player;
                NotifyStateChanged();
            });


        }

    }

}

