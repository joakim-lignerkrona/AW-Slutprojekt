using System.Runtime.Serialization;
using System.Xml.Linq;

namespace TriviaRoyale.Server.Models
{
    public class QandA
    {
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
    }
}
