using Microsoft.AspNetCore.SignalR;
using TriviaRoyale.Shared;

namespace TriviaRoyale.Server.Hubs
{
    public class QuizHub : Hub
    {
        public async Task SendAnswer(string answer)
        {
            await Clients.All.SendAsync("ReceiveAnswer", answer);
        }

        public async Task CreatePlayer(Player player)
        {
            await Clients.All.SendAsync("NewPlayer", player);
        }
        public async Task StartGame()
        {
            await Clients.All.SendAsync("StateChange", GameState.Playing);
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine("User connected");



            return base.OnConnectedAsync();
        }
    }
}
