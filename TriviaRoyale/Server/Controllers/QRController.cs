using Microsoft.AspNetCore.Mvc;
using System.Web;
using TriviaRoyale.Server.Models;

namespace TriviaRoyale.Server.Controllers
{
    public class QRController : Controller
    {
        QRService service;
        public QRController(QRService service)
        {
            this.service = service;
        }
        [HttpGet("/qr/{url}")]
        public IActionResult Index(string url)
        {
            var decodedUrl = HttpUtility.UrlDecode(url);
            return File(service.GetQRCode(decodedUrl), "image/png");
        }
    }
}
