using Microsoft.AspNetCore.SignalR.Client;

namespace TriviaRoyale.Client.Pages
{
    public partial class HostPage
    {


        protected override async Task OnInitializedAsync()
        {
            service.OnChange += StateHasChanged;

            if(!IsConnected)
            {
                await service.ConnectAsync();

            }
        }


        public bool IsConnected =>
            service.hubConnection?.State == HubConnectionState.Connected;

        public void Dispose()
        {
            service.OnChange -= StateHasChanged;
        }
    }
}
