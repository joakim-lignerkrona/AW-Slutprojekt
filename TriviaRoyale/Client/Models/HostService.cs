using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace TriviaRoyale.Client.Models
{
    public class HostService : DaddyService
    {
        public string HostID { get; set; }
        public HostService(NavigationManager Navigation) : base(Navigation)
        {
            hubConnection.On("StartGame", () =>
            {
                Console.WriteLine("Game Started");
            });
            hubConnection.On<string>("HostAccepted", (hostID) =>
            {
                this.HostID = hostID;
                NotifyStateChanged();
            });
        }

        public void ClearPlayerIsAnswering()
        {
            PlayerAnswering = null;
            NotifyStateChanged();
        }

    }
}
