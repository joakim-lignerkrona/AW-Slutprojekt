namespace TriviaRoyale.Shared
{
    public class GameRoom
    {
        public string Id { get; set; }
        public string HostID { get; set; }
        public List<Player> Players { get; set; } = new();
        public GameState GameState { get; set; }

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
        public void NewGame()
        {
            Players.ForEach(p => p.Points = 0);
        }
    }
}
