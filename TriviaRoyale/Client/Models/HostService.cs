using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Timers;
using TriviaRoyale.Shared;
using TriviaRoyale.Shared.Questions;

namespace TriviaRoyale.Client.Models
{
    public class HostService : DaddyService
    {
        private System.Timers.Timer timer;
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
            timer = new System.Timers.Timer(10000); // run every minute
            timer.Elapsed += (object sender, ElapsedEventArgs e) => GetPlayers();
            timer.Start();

        }

        public void ClearPlayerIsAnswering()
        {
            PlayerAnswering = null;
            NotifyStateChanged();
        }

    }
}
