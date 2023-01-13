using Microsoft.AspNetCore.Components;
using System.Text.Json;
using TriviaRoyale.Shared;
using TriviaRoyale.Shared.Questions;

namespace TriviaRoyale.Client.Shared.Host.QuestionScreen
{
    public partial class QuestionScreen
    {
        [Parameter]
        public Question question { get; set; }
        public bool CorrectAnswer { get; set; }

        async protected override void OnInitialized()
        {
            await GetQuestion();
        }        

        private void PlayerGuess(Question question)
        {
            throw new NotImplementedException();
        }

        private async Task PlayerGuess()
        {
            if (CorrectAnswer)
            {
                service.playerAnswering.Points++;
                //Koppla samman detta med listan av players?
                //Vänta på knapptryck av hosten för att komma tillbax
            }
            else
            {
                //Se till att denna spelare inte får gissa på DENNA frågan igen.
                //Vänta på knapptryck av hosten för att komma tillbax (igen)? eller? Countdown kanske?
                await PlayerGuess();
            }

        }

        async Task GetQuestion()
        {
            string url = navigation.BaseUri + "API/Question/";
            HttpClient httpClient = new();

            var q = await httpClient.GetAsync(url);
            if (q.IsSuccessStatusCode)
            {
                // Read the response content
                var content = await q.Content.ReadAsStringAsync();
                //// Deserialize the content into an object
                question = JsonSerializer.Deserialize<Question>(content);
                StateHasChanged();
            }
        }
    }
}
