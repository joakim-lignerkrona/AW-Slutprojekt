using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Text.Json;
using TriviaRoyale.Shared;
using TriviaRoyale.Shared.Questions;

namespace TriviaRoyale.Client.Shared.Host.QuestionScreen
{
    public partial class QuestionScreen
    {

        [Parameter]
        public bool CorrectAnswer { get; set; }
        public List<Question> questions { get; set; }
        public Question question { get; set; }
        public List<Question> hardQuestions { get; set; }


        async protected override void OnInitialized()
        {
            service.OnChange += StateHasChanged;
            await GetQuestions();
            GetQuestion();
            await GetHardQuestions();
        }

        //private async Task PlayerGuess()
        //{
        //    if(CorrectAnswer)
        //    {
        //        service.PlayerAnswering.Points++;
        //        //Koppla samman detta med listan av players?
        //        //Vänta på knapptryck av hosten för att komma tillbax
        //    }
        //    else
        //    {
        //        service.GameRoundPlayers.Remove(service.PlayerAnswering);
        //        //Se till att denna spelare inte får gissa på DENNA frågan igen.
        //        //Vänta på knapptryck av hosten för att komma tillbax (igen)? eller? Countdown kanske?
        //        //await PlayerGuess();
        //    }
        //}

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
                var json = JsonSerializer.Deserialize<Question[]>(content);
                questions = json.ToList();
                StateHasChanged();
            }
        }
        async Task GetHardQuestions()
        {
            string url = navigation.BaseUri + "api/Questions/";
            HttpClient httpClient = new();

            var q = await httpClient.GetAsync(url);
            if (q.IsSuccessStatusCode)
            {
                // Read the response content
                var content = await q.Content.ReadAsStringAsync();
                // Deserialize the content into an object
                var json = JsonSerializer.Deserialize<Question[]>(content);
                hardQuestions = json.ToList();
                StateHasChanged();
            }
        }

        void GetQuestion()
        {
            var index = Random.Shared.Next(0, questions.Count);
            question = questions[index];
            questions.RemoveAt(index);
            service.ClearPlayerIsAnswering();
            StateHasChanged();
        }
        void GetHardQuestion()
        {
            var index = Random.Shared.Next(0, questions.Count);
            question = hardQuestions[index];
            hardQuestions.RemoveAt(index);
            service.ClearPlayerIsAnswering();
        }

        async Task EndGame()
        {
            GetHardQuestion();
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
            service.PlayerAnswering.Points++;
            await service.hubConnection.InvokeAsync("CorrectAnswer", service.PlayerAnswering);
            service.ClearPlayerIsAnswering();
        }


        public void Dispose()
        {
            service.OnChange -= StateHasChanged;
        }
    }
}
