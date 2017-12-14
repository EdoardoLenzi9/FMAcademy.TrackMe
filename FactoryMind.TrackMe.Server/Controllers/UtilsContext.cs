using Microsoft.AspNetCore.Mvc;
using FactoryMind.TrackMe.Business.Services;
using System.Threading.Tasks;
using FactoryMind.TrackMe.Business.Containers;
using FactoryMind.TrackMe.Business.Utility;
using System;
using Newtonsoft.Json;
using FactoryMind.TrackMe.Domain.Models;
using System.Net.Http;

namespace FactoryMind.TrackMe.Server.Controllers
{
    [Route("/api/1/utils")]
    public class UtilsController : Controller
    {
        public UtilsController(UserService UserService, RoomService roomService, IAuthorizationContext authorizationContext)
        {        
        }

        [Route("google")]
        [HttpGet]
        public async Task<string> GetGoole()
        {
            var RequestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://www.google.it/search?q=dildo&rlz=1C1CHBF_enIT742IT742&oq=dildo&aqs=chrome..69i57j0l4j5.1327j0j8&sourceid=chrome&ie=UTF-8");
            var Client = new HttpClient();
            var Answer = await Client.SendAsync(RequestMessage);
            return await Answer.Content.ReadAsStringAsync();
        }

        [Route("ping")]
        [HttpGet]
        public string Ping()
        {
            return "Server Online";
        }
    }
}