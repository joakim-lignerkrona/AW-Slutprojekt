using System.Reflection;
using System.Text.Json;
using TriviaRoyale.Shared.Questions;

namespace TriviaRoyale.Server.Models
{

	public class QuestionService
	{
		List<Question> questions;
		List<Question> hardQuestions;

		public QuestionService()
		{
			string path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"JsonData/disney.json");
			string jsonString = File.ReadAllText(path);
			//string json = "{\"questions\":[{\"questionText\":\"What is the name of the princess in Sleeping Beauty?\",\"answer\":\"Princess Aurora\"},{\"questionText\":\"In The Lion King, what is the name of Simba's mother?\",\"answer\":\"Sarabi\"},{\"questionText\":\"What is the name of the friendly ghost in Disney's Haunted Mansion ride?\",\"answer\":\"Gus\"},{\"questionText\":\"What is the name of the pirate captain in the Pirates of the Caribbean ride?\",\"answer\":\"Captain Jack Sparrow\"},{\"questionText\":\"In The Little Mermaid, what is the name of Ariel's best friend?\",\"answer\":\"Flounder\"}]}";
			Questions listOfQuestions = JsonSerializer.Deserialize<Questions>(jsonString)!;
			//this.questions = new List<Question> {
			//	new Question { answer= "Princess Aurora", questionText = "What is the name of the princess in Sleeping Beauty?"},
			//	new Question { answer= "Sarabi", questionText = "In The Lion King, what is the name of Simba's mother?"},
			//	new Question { answer= "Gus", questionText = "What is the name of the friendly ghost in Disney's Haunted Mansion ride?"},
			//	new Question { answer= "Captain Jack Sparrow", questionText = "What is the name of the pirate captain in the Pirates of the Caribbean ride?"},
			//	new Question { answer= "Flounder", questionText = "In The Little Mermaid, what is the name of Ariel's best friend?"}
			//};

			//string hardPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"JsonData\HardQuestions.json");
			//string HardJsonString = File.ReadAllText(hardPath);
			//Questions tempQuestions = JsonSerializer.Deserialize<Questions>(HardJsonString)!;
			//this.hardQuestions = new List<Question>(tempQuestions.questions);
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
