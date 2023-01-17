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
            });

            hubConnection.On<string>("ClickerName", (btn) =>
            {
                PlayerToAnswer = btn;
                NotifyStateChanged();
            });


        }

    }

}

