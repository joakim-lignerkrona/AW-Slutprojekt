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
                var hostCookie = await cookie.GetValue("HostID");
                var roomCookie = await cookie.GetValue("RoomID");

                if(hostCookie == null || RoomId != roomCookie)
                {
                    var hostID = Guid.NewGuid().ToString();
                    await cookie.SetValue("HostID", hostID);
                    await cookie.SetValue("RoomID", RoomId);
                    await service.hubConnection.InvokeAsync("RegisterHost", hostID, RoomId);

                }
                else
                    await service.hubConnection.InvokeAsync("RegisterHost", null, RoomId);
            }
            service.GetPlayers();
        }


        public bool IsConnected =>
            service.hubConnection?.State == HubConnectionState.Connected;

        public void Dispose()
        {
            service.OnChange -= StateHasChanged;
        }
    }
}
