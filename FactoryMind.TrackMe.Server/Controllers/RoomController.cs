using Microsoft.AspNetCore.Mvc;
using FactoryMind.TrackMe.Business.Services;
using System.Threading.Tasks;
using FactoryMind.TrackMe.Business.Containers;
using FactoryMind.TrackMe.Business.Utility;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using AutoMapper;
using FactoryMind.TrackMe.Domain.Models;

namespace FactoryMind.TrackMe.Server.Controllers
{
    [Route("/api/1/room")]
    public class RoomController : Controller
    {
        private RoomService _roomService;
        private IAuthorizationContext _authorizationContext;

        public RoomController(RoomService roomService, IAuthorizationContext authorizationContext)
        {
            _roomService = roomService;
            _authorizationContext = authorizationContext;
        }

        [Route("opendefaultroom")]
        [HttpGet]
        public async Task<RoomDto> OpenDefaultRoom([FromHeader] string id)
        {
            return (await _roomService.OpenDefaultRoomAsync(_authorizationContext.User.Id)).AsDto();
        }

        [HttpPost("createroom/{roomName}")]
        public async Task<RoomDto> CreateRoom([FromHeader] string id, string roomName)
        {
            return (await _roomService.CreateRoomAsync(_authorizationContext.User.Id, roomName)).AsDto();
        }

        [Route("showroom")]
        [HttpGet]
        public async Task<List<RoomDto>> ShowRoom([FromHeader] string id)
        {
            return await _roomService.ShowRoomAsync(_authorizationContext.User.Id);
        }

        [HttpDelete("deleteroom/{roomName}")]
        public async Task DeleteRoom([FromHeader] string id, string roomName)
        {
            await _roomService.DeleteRoomAsync(_authorizationContext.User.Id, roomName);
        }

        [HttpPut("addperson/{userName}/toroom/{roomName}")]
        public async Task AddPersonToRoom([FromHeader] string id, string roomName, string userName)
        {
            await _roomService.AddPersonToRoomAsync(_authorizationContext.User.Id, roomName, userName);
        }

        [HttpPut("removeperson/{userName}/fromroom/{roomName}")]
        public async Task RemovePersonToRoom([FromHeader] string id, string roomName, string userName)
        {
            await _roomService.RemovePersonFromRoomAsync(_authorizationContext.User.Id, roomName, userName);
        }

        [HttpGet("getusers/{roomName}")]
        public async Task<List<UserDto>> GetUsersInRoom([FromHeader] string id, string roomName)
        {
            return (await _roomService.GetUsersInRoom(_authorizationContext.User.Id, roomName)).AsDto();
        }

        [HttpGet("isuser/adminof/{roomName}")]
        public async Task<bool> IsUserAdmin([FromHeader]string id, string roomName)
        {
            return await _roomService.IsUserAdminAsync(int.Parse(id), roomName);
        }

        [HttpGet("userrooms")]
        public async Task<List<RoomDto>> GetUserRooms([FromHeader]string id)
        {
            return (await _roomService.GetUserRooms(int.Parse(id))).AsDto();
        }
    }
}