using Microsoft.AspNetCore.Mvc;
using FactoryMind.TrackMe.Business.Services;
using System.Threading.Tasks;
using FactoryMind.TrackMe.Business.Containers;
using FactoryMind.TrackMe.Business.Utility;
using System;
using Newtonsoft.Json;
using FactoryMind.TrackMe.Domain.Models;
using System.Collections.Generic;

namespace FactoryMind.TrackMe.Server.Controllers
{
    [Route("/api/1/user")]
    public class UserController : Controller
    {
        private UserService _userService;
        private RoomService _roomService;
        private IAuthorizationContext _authorizationContext;

        public UserController(UserService UserService, RoomService roomService, IAuthorizationContext authorizationContext)
        {
            _userService = UserService;
            _roomService = roomService;
            _authorizationContext = authorizationContext;
        }

        [Route("registration")]
        [HttpPost]
        public async Task<UserDto> NewRegistration([FromHeader] string mail, [FromHeader] string password, [FromHeader] string gender)
        {
            //new DbReset();
            var user = await _userService.NewRegistrationAsync(mail, password, gender);
            if (!await _roomService.ExistRoomAsync("DefaultRoom"))
            {
                await _roomService.CreateRoomAsync(user.Id, "DefaultRoom");
            }
            await _roomService.SudoAddPersonToRoomAsync("DefaultRoom", mail);
            return user.AsDto();
        }

        [Route("userlist")]
        [HttpPost]
        public async Task<List<UserDto>> GetUserList([FromHeader] string id)
        {
            return (await _userService.GetUserList(int.Parse(id))).AsDto();
        }
    }
}