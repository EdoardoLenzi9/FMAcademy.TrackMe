using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FactoryMind.TrackMe.Business.Exceptions;
using FactoryMind.TrackMe.Business.Repos;
using FactoryMind.TrackMe.Domain.Models;

namespace FactoryMind.TrackMe.Business.Services
{
    public class RoomService
    {
        private IUserRepository uRepo;
        private IRoomRepository rRepo;

        public RoomService(IUserRepository us, IRoomRepository rr)
        {
            uRepo = us;
            rRepo = rr;
        }

        public async Task<bool> ExistRoomAsync(string roomName)
        {
            return await rRepo.ExistRoomAsync(roomName);
        }

        public async Task<bool> IsUserAdminAsync(int userId, string roomName)
        {
            return await rRepo.IsUserAdminAsync(userId, roomName);
        }

        public async Task<List<RoomDto>> ShowRoomAsync(int userId)
        {
            if (userId < 0)
            {
                throw new ParameterException("errore parametri in [ShowRoomAsync]");
            }
            var userRooms = await rRepo.GetUserRoomsAsync(userId);
            if (userRooms == null)
            {
                throw new NotFoundException("Utente non è in nessuna room [ShowRoomAsync]");
            }
            return userRooms.AsDto();
        }

        public async Task<Room> CreateRoomAsync(int userId, string roomName)
        {
            if (userId < 0 || String.IsNullOrEmpty(roomName))
            {
                throw new ParameterException("errore parametri in [CreateRoomAsync]");
            }
            if (!await uRepo.IsUserInDatabaseAsync(userId))
            {
                throw new GeneralException("errore utente non registrato [CreateRoomAsync]");
            }
            var storedRoom = await rRepo.GetRoomAsync(roomName);
            if (storedRoom != null)
            {
                throw new GeneralException("errore room già in database [CreateRoomAsync]");
            }
            var room = await rRepo.CreateRoomAsync(userId, roomName);
            if (room.RoomId < 0)
            {
                throw new RepositoryException("errore repo (roomId < 0) in [CreateRoomAsync]");
            }
            return room;
        }

        public async Task DeleteRoomAsync(int userId, string roomName)
        {
            if (userId < 0 || String.IsNullOrEmpty(roomName))
            {
                throw new ParameterException("errore parametri in [DeleteRoomAsync]");
            }
            var room = await rRepo.GetRoomAsync(roomName);
            if (room == null)
            {
                throw new NotFoundException("room non trovata in db [DeleteRoomAsync]");
            }
            if (userId != room.AdminId) // se non sono l'admin
            {
                throw new AuthorizationException("l'utente non può eseguire questa operazione [DeleteRoomAsync]");
            }
            if (room.AdminId < 0) // i can delete rooms only if i'm the admin
            {
                throw new GeneralException("room admin id = default value (-1) [DeleteRoomAsync]");
            }
            if (!await rRepo.DeleteRoomAsync(room.RoomId))
            {
                throw new RepositoryException("errore repo in [DeleteRoomAsync]");
            }
        }

        public async Task SudoAddPersonToRoomAsync(string roomName, string person)
        {
            if (String.IsNullOrEmpty(roomName) || String.IsNullOrEmpty(person))
            {
                throw new ParameterException("errore parametri in [AddPersonToRoomAsync]");
            }
            var room = await rRepo.GetRoomAsync(roomName);
            if (room == null)
            {
                throw new NotFoundException("errore in [AddPersonToRoomAsync]");
            }
            var userToAddId = await uRepo.GetUserAsync(person);
            if (userToAddId == null)
            {
                throw new NotFoundException("errore in [AddPersonToRoomAsync]");
            }
            if (room.UsersId.Any(id => id == userToAddId.Id))
            {
                throw new GeneralException("utente già nella room [AddPersonToRoomAsync]");
            }
            if (!await rRepo.AddUserAsync(room.RoomId, userToAddId.Id))
            {
                throw new RepositoryException("errore in [AddPersonToRoomAsync]");
            }
        }

        public async Task AddPersonToRoomAsync(int userId, string roomName, string person)
        {
            if (userId < 0 || String.IsNullOrEmpty(roomName) || String.IsNullOrEmpty(person))
            {
                throw new ParameterException("errore parametri in [AddPersonToRoomAsync]");
            }

            var room = await rRepo.GetRoomAsync(roomName);
            if (room == null)
            {
                throw new NotFoundException("errore in [AddPersonToRoomAsync]");
            }

            var userToAddId = await uRepo.GetUserAsync(person);

            if (userToAddId == null)
            {
                throw new NotFoundException("errore in [AddPersonToRoomAsync]");
            }

            if (room.UsersId.Any(id => id == userToAddId.Id))
            {
                throw new GeneralException("utente già nella room [AddPersonToRoomAsync]");
            }

            if (room.AdminId != userId)
            {
                throw new AuthorizationException("errore in [AddPersonToRoomAsync]");
            }
            if (!await rRepo.AddUserAsync(room.RoomId, userToAddId.Id))
            {
                throw new RepositoryException("errore in [AddPersonToRoomAsync]");
            }
        }

        public async Task RemovePersonFromRoomAsync(int userId, string roomName, string userName) // solo l'admin può rimuovere persona dal gruppo
        {
            if (userId < 0 || String.IsNullOrEmpty(roomName) || String.IsNullOrEmpty(userName))
            {
                throw new ParameterException("errore parametri in [RemovePersonFromRoomAsync]");
            }
            var room = await rRepo.GetRoomAsync(roomName);
            if (room == null)
            {
                throw new NotFoundException("room null [RemovePersonFromRoomAsync]");
            }
            var userToRemove = await uRepo.GetUserAsync(userName);
            if (userToRemove == null)
            {
                throw new NotFoundException("user null [RemovePersonFromRoomAsync]");
            }
            if (room.AdminId != userId)
            {
                throw new AuthorizationException("user isn't admin [RemovePersonFromRoomAsync]");
            }
            if (!await rRepo.RemoveUserAsync(room.RoomId, userToRemove.Id))
            {
                throw new Exception("errore in [RemovePersonFromRoomAsync]");
            }
        }

        public async Task<Room> OpenDefaultRoomAsync(int userId)
        {
            if (userId < 0)
            {
                throw new ParameterException("errore parametri in [OpenDefaultRoomAsync]");
            }
            var room = await rRepo.GetUserRoomsAsync(userId);
            if (room == null)
            {
                throw new NotFoundException("errore in [OpenDefaultRoomAsync]");
            }
            if (room.Count == 0)
            {
                throw new GeneralException("l'utente non è in nessuna room [OpenDefaultRoomAsync]");
            }
            return room.First();
        }

        public async Task<List<User>> GetUsersInRoom(int userId, string roomName)
        {
            if (userId < 0)
            {
                throw new ParameterException("errore parametri in [GetUsersInRoom]");
            }
            var room = await rRepo.GetRoomAsync(roomName);
            if (room == null)
            {
                throw new NotFoundException("errore in [GetUsersInRoom]");
            }
            return await rRepo.GetUsersInRoomAsync(room.RoomId);
        }

        public async Task<List<Room>> GetUserRooms(int userId)
        {
            if (userId < 0)
            {
                throw new ParameterException("errore parametri in [GetUsersRooms]");
            }
			var roomList = await rRepo.GetUserRoomsAsync(userId);
			if(roomList.Count==0)
			{
				return null;
			}
            return roomList;
        }
    }
}