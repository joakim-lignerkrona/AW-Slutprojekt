namespace TriviaRoyale.Client.Shared.PlayerPage
{
    public partial class Lobby
    {
        protected override async Task OnInitializedAsync()
        {
            service.OnChange += StateHasChanged;

        }

        public void Dispose()
        {
            service.OnChange -= StateHasChanged;
        }

    }
}
