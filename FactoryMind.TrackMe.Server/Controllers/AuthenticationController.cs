using Microsoft.AspNetCore.Mvc;
using FactoryMind.TrackMe.Business.Services;
using System.Threading.Tasks;
using FactoryMind.TrackMe.Business.Containers;
using FactoryMind.TrackMe.Business.Utility;
using System;
using Newtonsoft.Json;
using FactoryMind.TrackMe.Domain.Models;

namespace FactoryMind.TrackMe.Server.Controllers
{
    [Route("/api/1/authentication")]
    public class AuthenticationController : Controller
    {
        private AuthenticationService _authenticationService;
        private IAuthorizationContext _authorizationContext;
        
        public AuthenticationController(AuthenticationService authenticationService, IAuthorizationContext authorizationContext)
        {
            _authenticationService = authenticationService;
            _authorizationContext = authorizationContext;
        }
        
        [Route("login")]
        [HttpPost]
        public async Task<UserDto> Login([FromHeader] string mail, [FromHeader] string password){
            return (await _authenticationService.LoginAsync(mail, password)).AsDto();
            //return JsonConvert.SerializeObject(user.AsDto());
        }
    }
}
