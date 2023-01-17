using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using TriviaRoyale.Shared;

namespace TriviaRoyale.Client.Models
{
    public class HostService : DaddyService
    {
        public List<Player> DiscardedPlayers { get; set; }
        public HostService(NavigationManager Navigation) : base(Navigation)
        {
            hubConnection.On("StartGame", () =>
            {
                Console.WriteLine("Game Started");
            });
        }

    }
}
