using System.Text.Json;
using TriviaRoyale.Server.Controllers;
using TriviaRoyale.Shared.Questions;

namespace TriviaRoyale.Server.Models
{
    public class DataService
    {
        //public List<QandA> ListOfQAndA { get; set; }

        //ListOfQAndA i framtiden..
        //ListOfQAndA = questionController.GetList();

        public bool Correct { get; set; }
        private readonly QuestionControllerAPI questionController;

        public string GetQuestion()
        {
            Random random = new Random();
            int r = random.Next(1, questionController.GetList().Count);
            return GetRandomQuestion(r);
        }
        private string GetRandomQuestion(int r) => questionController.GetList()[r].Question;
    }
}
