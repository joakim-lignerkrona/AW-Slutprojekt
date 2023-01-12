using TriviaRoyale.Shared;

namespace TriviaRoyale.Client.Models
{
    public class HostService
    {
        public string RoomID { get; set; }
        public List<Player> Players { get; set; }

        //public string QuestionText { get; set; }
        //public string Answer { get; set; }

    }
}
