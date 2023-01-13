using System.Text.Json;
using TriviaRoyale.Shared.Questions;

namespace TriviaRoyale.Server.Models
{

    public class DataService
    {
        List<int> usedNumbers { get; set; } = new List<int>();
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
            int r = GetRandomNumber();

            if (usedNumbers.Any(n => n == r))
            {
                GetQuestion();
            }
            usedNumbers.Add(r);
            return GetRandomQuestion(r);
        }

        private int GetRandomNumber()
        {
            Random random = new Random();
            int r = random.Next(1, questions.Count);
            return r;
        }

        private Question GetRandomQuestion(int r) => questions[r];
    }
}
