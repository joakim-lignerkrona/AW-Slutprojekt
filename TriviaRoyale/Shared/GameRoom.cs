namespace TriviaRoyale.Shared
{
    public class GameRoom
    {
        public string Id { get; set; }
        public List<Player> Players { get; set; } = new();

        public GameRoom()
        {
            Id = System.Guid.NewGuid().ToString();
        }
        public void AddPlayer(Player player)
        {
            Players.Add(player);
        }
        public void RemovePlayer(Player player)
        {
            Players.Remove(player);
        }

    }
}
