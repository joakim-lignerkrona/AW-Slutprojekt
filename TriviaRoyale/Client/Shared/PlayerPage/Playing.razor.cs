using Microsoft.AspNetCore.SignalR.Client;

namespace TriviaRoyale.Client.Shared.PlayerPage
{
    public partial class Playing
    {
        private async Task Answer()
        {
            Random rand = new Random();
            int choice = rand.Next(1, 3);

            if (choice == 1)
            {
                await service.hubConnection.SendAsync("AnswerButton1");
            }
            else
            {
                await service.hubConnection.SendAsync("AnswerButton2");
            }
        }
    }
}
