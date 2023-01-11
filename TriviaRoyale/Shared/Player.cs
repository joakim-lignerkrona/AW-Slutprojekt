namespace TriviaRoyale.Shared
{
    public class Player
    {
        public static int createdPlayers;
        public int Points { get; set; }
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
