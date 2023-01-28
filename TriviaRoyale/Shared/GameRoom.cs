using System.Timers;

namespace TriviaRoyale.Shared
{
	public class GameRoom
	{
		private System.Timers.Timer timer;
		public string Id { get; set; }
		public string HostID { get; set; }
		public List<Player> Players { get; set; } = new();
		public GameState GameState { get; set; }
		public Player PlayerAnswering { get; set; }

		public Action OnChange { get; set; }

		public GameRoom()
		{
			Id = System.Guid.NewGuid().ToString();
			timer = new System.Timers.Timer(1000); // run every minute
			timer.Elapsed += new ElapsedEventHandler(CheckForDisconnectedUsers);
			timer.Start();
		}

		private void CheckForDisconnectedUsers(object sender, ElapsedEventArgs e)
		{
			var playersCount = Players.Count;
			lock(Players)
			{
				Players.RemoveAll(p => p.InactiveSince?.AddSeconds(30) < DateTime.Now);
			}
			//if(playersCount != Players.Count)
			//{
			//    OnChange?.Invoke();
			//}
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
			PlayerAnswering = null;
			Players.RemoveAll(p => !p.isActive);
			Players.ForEach(p => p.Points = 0);
		}
		public void NewQuestion()
		{
			PlayerAnswering = null;
		}

		public void PlayerDidAnswer(Player player)
		{

			if(PlayerAnswering == null)
			{
				PlayerAnswering = player;
			}


		}

		public void OpenQuestion()
		{
			PlayerAnswering = null;
		}

	}
}
