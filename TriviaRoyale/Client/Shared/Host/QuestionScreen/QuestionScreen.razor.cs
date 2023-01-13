using Microsoft.AspNetCore.Components;
using System.Text.Json;
using TriviaRoyale.Shared.Questions;

namespace TriviaRoyale.Client.Shared.Host.QuestionScreen
{
    public partial class QuestionScreen
    {
        [Parameter]
        public Question question { get; set; }

        async protected override void OnInitialized()
        {
            //TODO
            //Fixa så att inte samma fråga kommer två gånger
            //GetRandomQuestion();
            //Flytta ut nedan till metoden.
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
        //async Task<Question> GetQuestion()
        //{
        //    QuestionText = "Vad heter Egyptens motsvarighet till Hesa Fredrik?";
        //    Answer = "Tutan Khamon..";
        //    return question;
        //}
    }
}
