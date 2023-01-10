using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TriviaRoyale.Server.Models;

namespace TriviaRoyale.Server.Controllers
{
    [Route("api/[controller]")]
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
        public QuestionControllerAPI(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.configuration = configuration;
        }

        //CLIENT SIDE
        [HttpGet]
        public List<QandA> GetList()
        {
            List<QandA> list = new List<QandA>();
            using (StreamReader reader = new StreamReader($"{webHostEnvironment.ContentRootPath}/jsondata/disney.json"))
            {
                list = JsonConvert.DeserializeObject<List<QandA>>(reader.ReadToEnd());
            }
            return list;
        }




        //SERVER SIDE
        //private void InsertList(List<QandA> list) 
        //{
        //    Hitta connection strängen!

        //}

    }
}
