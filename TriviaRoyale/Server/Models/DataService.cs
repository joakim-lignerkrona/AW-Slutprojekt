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
    }
}
