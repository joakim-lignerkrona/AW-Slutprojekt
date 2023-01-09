using Microsoft.AspNetCore.SignalR;

namespace TriviaRoyale.Server.Hubs
{
    public class QuizHub : Hub
    {
        public async Task SendAnswer(string answer)
        {
            await Clients.All.SendAsync("ReceiveAnswer", answer);
        }

        public override Task OnConnectedAsync()
        {


            return base.OnConnectedAsync();
        }
    }
}
