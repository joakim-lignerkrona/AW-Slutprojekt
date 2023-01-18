using System.ComponentModel.DataAnnotations;

namespace TriviaRoyale.Shared
{
    public class Player
    {
        public static int createdPlayers;
        public string ID { get; set; }
        public string RoomID { get; set; }
        public string Emoji { get; set; }

        public int Points { get; set; }

        [Required(ErrorMessage = "You must enter a name")]
        public string Name { get; set; }

        public Player()
        {
            createdPlayers++;
        }
        public Player(string name)
        {
            this.Name = name;
        }
    }
}
