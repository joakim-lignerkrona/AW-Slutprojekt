using Microsoft.AspNetCore.SignalR.Client;

namespace TriviaRoyale.Client.Shared.PlayerPage
{
    public partial class Lobby
    {
        protected override async Task OnInitializedAsync()
        {
            service.OnChange += StateHasChanged;
            await service.hubConnection.InvokeAsync("GetConnectedPlayers", service.RoomID);
        }

        public void Dispose()
        {
            service.OnChange -= StateHasChanged;
        }

    }
}
