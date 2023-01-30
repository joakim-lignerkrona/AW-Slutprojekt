using System.Reflection;
using System.Text.Json;
using TriviaRoyale.Shared.Questions;

namespace TriviaRoyale.Server.Models
{

    public class QuestionService
    {
        List<Question> questions;
        //List<Question> hardQuestions;

        public QuestionService()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"JsonData/Disney.json");
            Console.WriteLine(path);
            string jsonString = File.ReadAllText(path);
            string json = "{\"questions\":[{\"questionText\":\"What is the name of the princess in Sleeping Beauty?\",\"answer\":\"Princess Aurora\"},{\"questionText\":\"In The Lion King, what is the name of Simba's mother?\",\"answer\":\"Sarabi\"},{\"questionText\":\"What is the name of the friendly ghost in Disney's Haunted Mansion ride?\",\"answer\":\"Gus\"},{\"questionText\":\"What is the name of the pirate captain in the Pirates of the Caribbean ride?\",\"answer\":\"Captain Jack Sparrow\"},{\"questionText\":\"In The Little Mermaid, what is the name of Ariel's best friend?\",\"answer\":\"Flounder\"}]}";
            Questions listOfQuestions = JsonSerializer.Deserialize<Questions>(jsonString)!;
            this.questions = listOfQuestions.questions.ToList();
            //        this.questions = new List<Question> {
            ////new Question{questionText = "Which historical king is believed to be portraited as 'King of Hearts'?", answer = "Charlemagne" },
            ////new Question{questionText = "Order these in the correct chronological order:" + "1) Wheels on luggage" + "2) First man on the moon" + "3) " + "The formal split up of Beatles", answer = "1, 2, 3" },
            ////new Question{questionText = "Who wrote the music for Backstreet Boys major hit 'Everybody'?", answer = "Junior - Liquid Love" },
            ////new Question {questionText = "Who played 'Jum-Jum' and 'Benke' in Mio min Mio?", answer = "Christian Bale"},
            ////new Question {questionText = "Which fruit shares 50% of their DNA with humans?", answer = "Banana"},
            ////new Question {questionText = "Which notes will be heard of you play low C (C2) on piano? Keep it withing first three octaves.", answer = "C, C, G, C, E, G, Bb, C"},
            ////new Question {questionText = "How many stars are there in the Chinese flag?", answer = "5"},
            ////new Question {questionText = "How many timezones are there in China?", answer = "1"},
            ////new Question {questionText = "In 1954, 'Lord of the Rings' was released. This story features four hobbits. What is the name of the Prime Minister of Sweden? ", answer = "Ulf Kristersson"},
            ////new Question {questionText = "Which color is in the middle of the rainbow?", answer = "Green" },
            //new Question {questionText = "Which fictional protagonist can get out of all kinds of tricky situations using his wits, a swiss army knife and some duct tape?", answer = "Angus MacGuyver"},
            //            new Question {questionText = "What is the appropriate clothing as a consultant?", answer = "Anything, paired a vest."},
            //            new Question {questionText = "Name one person that has beaten Pontus Wittenmark with 6-0 in table tennis.", answer = "Magnus Engdahl"},
            //            new Question {questionText = "Which programming language has a wacko protocol, according to Pontus Wittenmark?", answer = "Javascript!"},
            //            new Question {questionText = "What is the best meal for a surprise lunch lecture?", answer = "Raw Chicken" }

            //            };

            //string hardPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"JsonData\HardQuestions.json");
            //string HardJsonString = File.ReadAllText(hardPath);
            //Questions tempQuestions = JsonSerializer.Deserialize<Questions>(HardJsonString)!;
            //this.hardQuestions = new List<Question>(tempQuestions.questions);
        }

        public Question[] GetQuestions()
        {

            return questions.ToArray();
        }
        //public Question[] GetHardQuestions()
        //{

        //	return hardQuestions.ToArray();
        //}
    }
}
