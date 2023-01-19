using Microsoft.AspNetCore.Mvc;
using TriviaRoyale.Server.Models;
using TriviaRoyale.Shared.Questions;

namespace TriviaRoyale.Server.Controllers
{
    [Route("api/")]
    public class QuestionController : Controller
    {

        private readonly QuestionService service;
        ////Klienten
        //private readonly IWebHostEnvironment webHostEnvironment;
        ////Servern
        //private readonly IConfiguration configuration;

        //DI
        public QuestionController(QuestionService service /*IWebHostEnvironment webHostEnvironment, IConfiguration configuration*/)
        {
            this.service = service;
            //this.webHostEnvironment = webHostEnvironment;
            //this.configuration = configuration;
        }

        //CLIENT SIDE
        [HttpGet("questions")]
        public Question[] GetList()
        {
            return service.GetQuestions();
        }

        //[HttpGet("hardquestions")]
        //public Question[] GetHardList()
        //{
        //	return service.GetHardQuestions();
        //}
    }
}
