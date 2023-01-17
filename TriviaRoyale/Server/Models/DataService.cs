using System.Text.Json;
using TriviaRoyale.Shared.Questions;

namespace TriviaRoyale.Server.Models
{

    public class DataService
    {
        List<Question> questions;
        List<Question> hardQuestions;

        public DataService()
        {
            string path = Path.Combine(Environment.CurrentDirectory, @"JsonData\disney.json");
            string jsonString = File.ReadAllText(path);
            Questions listOfQuestions = JsonSerializer.Deserialize<Questions>(jsonString)!;
            this.questions = new List<Question>(listOfQuestions.questions);

            string hardPath = Path.Combine(Environment.CurrentDirectory, @"JsonData\HardQuestions.json");
            string HardJsonString = File.ReadAllText(hardPath);
            Questions tempQuestions = JsonSerializer.Deserialize<Questions>(HardJsonString)!;
            this.hardQuestions = new List<Question>(tempQuestions.questions);
        }

        public Question[] GetQuestions()
        {

            return questions.ToArray();
        }        
        public Question[] GetHardQuestions()
        {

            return hardQuestions.ToArray();
        }
    }
}
