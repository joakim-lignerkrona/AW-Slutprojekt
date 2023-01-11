using Microsoft.AspNetCore.SignalR.Client;

namespace TriviaRoyale.Client.Pages
{
    public partial class HostPage
    {
        private HubConnection? hubConnection;
        private List<string> messages = new List<string>();
        private string? userInput;
        private string? messageInput;

        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(Navigation.ToAbsoluteUri("/Quiz"))
                .Build();

            hubConnection.On<string, string>("ReceiveAnswer", (user, answer) =>
            {
                var encodedMsg = $"{answer}";
                messages.Add(encodedMsg);
                StateHasChanged();
            });

            await hubConnection.StartAsync();
        }

        private async Task Send()
        {
            if(hubConnection is not null)
            {
                await hubConnection.SendAsync("SendMessage", userInput, messageInput);
            }
        }

        public bool IsConnected =>
            hubConnection?.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if(hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }
    }
}
