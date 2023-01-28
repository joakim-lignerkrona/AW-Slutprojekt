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
            //await GetHardQuestions();
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
                var json = JsonSerializer.Deserialize<Question[]>(content);
                questions = json.ToList();
                StateHasChanged();
            }
        }

        void GetQuestion()
        {
            int index;
            var localQuestions = service.GameState == GameState.EliminationRound ? hardQuestions : questions;
            index = Random.Shared.Next(0, localQuestions.Count);
            question = localQuestions[index];
            localQuestions.RemoveAt(index);

           
            service.NewQuestion();
            StateHasChanged();
        }
        async Task GetHardQuestions()
        {
            string url = navigation.BaseUri + "api/hardquestions/";
            HttpClient httpClient = new();

            var q = await httpClient.GetAsync(url);
            if(q.IsSuccessStatusCode)
            {
                // Read the response content
                var content = await q.Content.ReadAsStringAsync();
                // Deserialize the content into an object
                var json = JsonSerializer.Deserialize<Question[]>(content);
                hardQuestions = json.ToList();
                StateHasChanged();
            }
        }


        async Task EndOrEliminate()
        {
            
            await service.hubConnection.InvokeAsync("EndGameOrEliminationRound");
        }
        async Task EndGame()
        {
            
            await service.hubConnection.InvokeAsync("EndOfGame");
        }
        async Task HandleWrongAnswer()
        {
            await service.hubConnection.InvokeAsync("WrongAnswer", service.PlayerAnswering);
            
        }
        async Task HandleCorrectAnswer()
        {
            await service.hubConnection.InvokeAsync("CorrectAnswer", service.PlayerAnswering);
            
        }


        public void Dispose()
        {
            service.OnChange -= StateHasChanged;
        }
    }
}
