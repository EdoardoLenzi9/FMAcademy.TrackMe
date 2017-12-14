using System;
using System.Threading.Tasks;
using FactoryMind.TrackMe.Business.Events;
using FactoryMind.TrackMe.Business.Repos;
using FactoryMind.TrackMe.Business.Containers;
using FactoryMind.TrackMe.Domain.Models;
using FactoryMind.TrackMe.Business.Exceptions;
using System.Collections.Generic;

namespace FactoryMind.TrackMe.Business.Services
{
    public class PositionService
    {
        private IPositionReposotory _positionRepo;
        private IRoomRepository _roomRepo;

        public PositionService(IPositionReposotory pr, IRoomRepository rr)
        {
            _positionRepo = pr;
            _roomRepo = rr;
        }

        public async Task AddPositionAsync(int id, float X, float Y)
        {
            await _positionRepo.AddPositionAsync(id, X, Y);
        }

        public async Task<List<Position>> GetPoints(int userId, int roomId)
        {
            var positions = new List<Position>();
            if (userId < 0 || roomId < 0)
            {
                throw new ParameterException("errore parametri in [GetPoints]");
            }
            var room = await _roomRepo.GetRoomAsync(roomId);
            if (room == null)
            {
                throw new RepositoryException("errore GetRoomAsync in [GetPoints]");
            }
            var users = await _roomRepo.GetUsersInRoomAsync(roomId);
            foreach (var x in users)
            {
                var p = await _positionRepo.GetPositionAsync(x.Id);
                if (p == null)
                {
                    throw new RepositoryException("errore GetPositionAsync in [GetPoints]");
                }
                positions.Add(p);
            }
            return positions;
        }
    }
}

