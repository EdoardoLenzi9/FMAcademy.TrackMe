using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FactoryMind.TrackMe.Domain.Models;

namespace FactoryMind.TrackMe.Business.Repos
{
    public sealed class RoomRepository : IRoomRepository
    {
        private List<Room> _rooms = new List<Room>();
        private UserRepository _userRepository;

        public RoomRepository(UserRepository userRepository){
            _userRepository = userRepository;
        }

        public Task<List<User>> GetUsersInRoomAsync(int roomId)
        {
            return Task.Run(() =>
             {
                 var room = _rooms.Find(r => r.RoomId == roomId);
                 var users = new List<User>();
                 room.UsersId.ForEach(async u => {
                     var user = await _userRepository.GetUserAsync(u);
                     users.Add(user);
                 });
                 return users;
             });
        }

        public Task<bool> ExistRoomAsync(string roomName)
        {
            return Task.Run(() =>
             {
                 return _rooms.Any(x => x.Name == roomName);
             });
        }

        public Task<Room> GetRoomAsync(int userId, int roomId)
        {
            return Task.Run(() =>
            {
                return _rooms.Find(x => x.AdminId == userId && x.RoomId == roomId);
            });
        }

        public Task<bool> AddUserAsync(int roomId, int userId)
        {
            return Task.Run(() =>
            {
                _rooms.Single(r => r.RoomId == roomId).UsersId.Add(userId);
                //Thread.Sleep(400); // simulazione ritardo server
                return true;
            });
        }

        public Task<Room> CreateRoomAsync(int adminId, string name)
        {
            return Task.Run(() =>
            {
                var _lastCreatedRoomId = 1;
                if (_rooms.Count != 0)
                {
                    _lastCreatedRoomId = _rooms.Select(room => room.RoomId).Max() + 1;
                }
                var userList = new List<int>();
                userList.Add(adminId);
                var newRoom = new Room { RoomId = _lastCreatedRoomId, AdminId = adminId, Name = name, UsersId = userList };
                _rooms.Add(newRoom);
                return newRoom;
            });
        }

        public Task<bool> DeleteRoomAsync(int id)
        {
            return Task.Run(() =>
            {
                _rooms.Remove(_rooms.Single(r => r.RoomId == id));
                //Thread.Sleep(400); // simulazione ritardo server
                return true;
            });
        }

        public Task<bool> DeleteRoomAsync(string name)
        {
            return Task.Run(() =>
            {
                _rooms.Remove(_rooms.Single(r => r.Name == name));
                //Thread.Sleep(400); // simulazione ritardo server
                return true;
            });
        }

        public Task<List<Room>> GetUserRoomsAsync(int userId)
        {
            return Task.Run(() =>
            {
                var roomsToReturn = new List<Room>();
                foreach (var r in _rooms)
                {
                    if (r.UsersId.Any(id => id == userId))
                    {
                        roomsToReturn.Add(r);
                    }
                }
                return roomsToReturn;
            });
        }

        public Task<bool> RemoveUserAsync(int roomId, int id)
        {
            return Task.Run(() =>
            {
                var userIds = _rooms.Single(r => r.RoomId == roomId).UsersId;
                userIds.Remove(userIds.Single(userid => userid == id));
                return true;
            });
        }

        public Task<bool> UpdateRoomAsync(int id, string newName)
        {
            return Task.Run(() =>
            {
                _rooms.Single(r => r.RoomId == id).Name = newName;
                return true;
            });
        }

        public Task<Room> GetRoomAsync(int roomId)
        {
            return Task.Run(() =>
            {
                return _rooms.Single(r => r.RoomId == roomId);
            });
        }

        public Task<Room> GetRoomAsync(string roomName)
        {
            return Task.Run(() =>
            {
                return _rooms.Find(r => r.Name == roomName);
            });
        }

        public Task<bool> IsUserAdminAsync(int userId, string roomName)
        {
            throw new NotImplementedException();
        }
    }
}