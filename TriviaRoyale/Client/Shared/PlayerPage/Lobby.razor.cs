using Microsoft.AspNetCore.Components;

namespace TriviaRoyale.Client.Shared.PlayerPage
{
    public partial class Lobby
    {
        [Parameter] public string Name { get; set; }
        protected override void OnInitialized()
        {
            // You can use the name here as well
        }

        private int countdown = 5;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if(firstRender)
            {
                while(countdown > 0)
                {
                    await Task.Delay(1000); // 1 second delay
                    countdown--;
                    StateHasChanged(); // Forces the component to re-render
                }
                Navigation.NavigateTo("/Question");
            }
        }

    }
}
