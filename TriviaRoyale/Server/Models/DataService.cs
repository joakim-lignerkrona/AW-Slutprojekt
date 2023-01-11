using System.Text.Json;
using TriviaRoyale.Shared.Questions;

namespace TriviaRoyale.Server.Models
{
    public class DataService
    {
        //public List<QandA> ListOfQAndA { get; set; }

        //ListOfQAndA i framtiden..
        //ListOfQAndA = questionController.GetList();

        List<Question> questions;

        public DataService()
        {
            string path = Path.Combine(Environment.CurrentDirectory, @"JsonData\disney.json");
            string jsonString = File.ReadAllText(path);
            Questions listOfQuestions = JsonSerializer.Deserialize<Questions>(jsonString)!;
            this.questions = new List<Question>(listOfQuestions.questions);
        }

        public Question[] GetQuestions()
        {

            return questions.ToArray();
        }
        public Question GetQuestion()
        {
            Random random = new Random();
            int r = random.Next(1, questions.Count);
            return GetRandomQuestion(r);
        }
        private Question GetRandomQuestion(int r) => questions[r];
    }
}
