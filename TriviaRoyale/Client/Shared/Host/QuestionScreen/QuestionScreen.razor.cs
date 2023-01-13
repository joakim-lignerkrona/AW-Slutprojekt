using Microsoft.AspNetCore.Components;
using System.Text.Json;
using TriviaRoyale.Client.Models;
using TriviaRoyale.Shared.Questions;


namespace TriviaRoyale.Client.Shared.Host.QuestionScreen
{
    public partial class QuestionScreen
    {
        
        [Parameter]
           public bool CorrectAnswer { get; set; }
        public Question question { get; set; }

        async protected override void OnInitialized()
        {
            
            GetQuestion();
            PlayerGuess();


        }

        private void PlayerGuess()
        {
            if (CorrectAnswer)
            {
                //Lägg till poäng till spelare 
            }
        }

        async Task GetQuestion()
        {
            string url = navigation.BaseUri + "API/Question/";
            HttpClient httpClient = new();
            
            var q = await httpClient.GetAsync(url);
            if(q.IsSuccessStatusCode)
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
