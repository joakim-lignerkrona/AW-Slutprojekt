using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace TriviaRoyale.Shared
{
    public class GameHost
    {
        public string Id{ get; set; }
        public List<Player> Players { get; set; } = new();

        public GameHost()
        {
            Id = System.Guid.NewGuid().ToString();
        }

    }
}
