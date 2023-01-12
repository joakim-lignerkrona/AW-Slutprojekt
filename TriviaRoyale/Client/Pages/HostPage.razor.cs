using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace TriviaRoyale.Client.Pages
{
    public partial class HostPage
    {

        [Parameter]
        public string RoomId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            service.OnChange += StateHasChanged;

            if(!IsConnected)
            {
                await service.ConnectAsync();
                await service.hubConnection.InvokeAsync("JoinRoom", RoomId);
                service.RoomID = RoomId;

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
