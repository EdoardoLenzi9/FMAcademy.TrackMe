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
    [Route("/api/1/position")]
    public class PositionController : Controller
    {
        private PositionService _positionService;
        private IAuthorizationContext _authorizationContext;

        public PositionController(PositionService positionService, IAuthorizationContext authorizationContext)
        {
            _positionService = positionService;
            _authorizationContext = authorizationContext;
        }

        [Route("addposition")]
        [HttpPost]
        public async Task AddPosition([FromHeader] string id, [FromBody] PositionDto position)
        {
            await _positionService.AddPositionAsync(_authorizationContext.User.Id, position.X, position.Y);
        }

        [Route("getpoints")]
        [HttpPost]
        public async Task<List<PositionDto>> GetPoints([FromHeader] string id, [FromBody] RoomDto room)
        {
            Console.WriteLine($"\n\nGetPoints {room.RoomId}\n\n");
            return (await _positionService.GetPoints(_authorizationContext.User.Id, room.RoomId)).AsDto();
        }
    }
}