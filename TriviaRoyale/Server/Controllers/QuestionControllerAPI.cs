using Microsoft.AspNetCore.Mvc;
using TriviaRoyale.Server.Models;
using TriviaRoyale.Shared.Questions;

namespace TriviaRoyale.Server.Controllers
{
    [Route("api/")]
    [ApiController]
    public class QuestionControllerAPI : ControllerBase
    {

        //TODO 
        /* 
        
        Serverlogiken och connectionsträng. Osäker på detta.
         
         
         */


        private readonly DataService service;
        //Klienten
        private readonly IWebHostEnvironment webHostEnvironment;
        //Servern
        private readonly IConfiguration configuration;

        //DI
        public QuestionControllerAPI(DataService service, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            this.service = service;
            this.webHostEnvironment = webHostEnvironment;
            this.configuration = configuration;
        }

        //CLIENT SIDE
        [HttpGet("questions")]
        public Question[] GetList()
        {

            return service.GetQuestions();
        }




        //SERVER SIDE
        //private void InsertList(List<QandA> list) 
        //{
        //    Hitta connection strängen!

        //}

    }
}
