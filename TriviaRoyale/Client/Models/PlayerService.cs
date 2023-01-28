using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using TriviaRoyale.Shared;

namespace TriviaRoyale.Client.Models
{
    public class PlayerService : DaddyService
    {


        public Player Player { get; set; }
        public IJSRuntime JsRuntime { get; }

        public PlayerService(NavigationManager Navigation, IJSRuntime jsRuntime) : base(Navigation)
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
                if(player.ID == Player.ID)
                {
                    JsRuntime.InvokeVoidAsync("playSound", "success-1.mp3");
                }
                NotifyStateChanged();
            });
            JsRuntime = jsRuntime;

            JsRuntime.InvokeVoidAsync("loadSound", "success-1.mp3");
        }

    }

}

