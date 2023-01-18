using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Text.Json;
using TriviaRoyale.Shared.Questions;

namespace TriviaRoyale.Client.Shared.Host.QuestionScreen
{
    public partial class QuestionScreen
    {

        [Parameter]
        public bool CorrectAnswer { get; set; }
        public List<Question> questions { get; set; }
        public Question question { get; set; }


        async protected override void OnInitialized()
        {
            service.OnChange += StateHasChanged;
            await GetQuestions();
        }

        private void PlayerGuess(Question question)
        {
            throw new NotImplementedException();
        }

        private async Task PlayerGuess()
        {
            if(CorrectAnswer)
            {
                service.PlayerAnswering.Points++;
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

        async Task GetQuestions()
        {
            string url = navigation.BaseUri + "api/Questions/";
            HttpClient httpClient = new();

            var q = await httpClient.GetAsync(url);
            if(q.IsSuccessStatusCode)
            {
                // Read the response content
                var content = await q.Content.ReadAsStringAsync();
                //// Deserialize the content into an object
                ///
                var json = JsonSerializer.Deserialize<Question[]>(content);
                questions = json.ToList();
                StateHasChanged();
            }
        }

        async Task GetQuestion()
        {
            var index = Random.Shared.Next(0, questions.Count);
            question = questions[index];
            questions.RemoveAt(index);
            service.ClearPlayerIsAnswering();
            StateHasChanged();
        }

        async Task EndGame()
        {
            service.ClearPlayerIsAnswering();
            await service.hubConnection.InvokeAsync("EndOfGame");
        }
        async Task HandleWrongAnswer()
        {
            await service.hubConnection.InvokeAsync("WrongAnswer", service.PlayerAnswering);
            service.ClearPlayerIsAnswering();
        }
        async Task HandleCorrectAnswer()
        {
            await service.hubConnection.InvokeAsync("CorrectAnswer", service.PlayerAnswering);
            service.ClearPlayerIsAnswering();
        }


        public void Dispose()
        {
            service.OnChange -= StateHasChanged;
        }
    }
}
