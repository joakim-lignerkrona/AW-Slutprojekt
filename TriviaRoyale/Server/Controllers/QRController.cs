using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Web;
using TriviaRoyale.Server.Hubs;
using TriviaRoyale.Server.Models;

namespace TriviaRoyale.Server.Controllers
{
    public class QRController : Controller
    {
        QRService service;
        IHubContext<QuizHub> hubContext;
        public QRController(QRService service, IHubContext<QuizHub> hubContext)
        {
            this.service = service;
            this.hubContext = hubContext;
        }
        [HttpGet("/qr/{url}")]
        public IActionResult Index(string url)
        {
            hubContext.Clients.All.SendAsync("ReceiveAnswer", $"{DateTime.Now.ToShortTimeString()}");
            var decodedUrl = HttpUtility.UrlDecode(url);
            return File(service.GetQRCode(decodedUrl), "image/png");
        }
    }
}
