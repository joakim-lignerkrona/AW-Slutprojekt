using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Web;
using TriviaRoyale.Server.Hubs;
using TriviaRoyale.Server.Models;
using TriviaRoyale.Shared;

namespace TriviaRoyale.Server.Controllers
{
	public class QRController : Controller
	{
		QRService service;
		IHubContext<QuizHub> hubContext;
		private readonly RoomService roomService;


		public QRController(QRService service, IHubContext<QuizHub> hubContext, RoomService roomService, )
		{
			this.service = service;
			this.hubContext = hubContext;
			this.roomService = roomService;

		}
		[HttpGet("/qr/{url}")]
		public IActionResult QR(string url)
		{
			hubContext.Clients.All.SendAsync("ReceiveAnswer", $"{DateTime.Now.ToShortTimeString()}");
			var decodedUrl = HttpUtility.UrlDecode(url);
			return File(service.GetQRCode(decodedUrl), "image/png");
		}

		[HttpGet("/")]
		public IActionResult Index()
		{
			GameRoom host = roomService.NewRoom();

			return Redirect($"/Host/{host.Id}");
		}
		[HttpGet("rooms")]
		public GameRoom[] GetRooms()
		{

			return roomService.rooms.ToArray();
		}
		[HttpGet("questions")]
		public IActionResult GetList()
		{
			return Ok("dataService.GetQuestions()");
		}
	}
}
