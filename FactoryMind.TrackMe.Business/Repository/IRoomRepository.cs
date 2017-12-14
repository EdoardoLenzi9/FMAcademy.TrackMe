using System.Collections.Generic;
using System.Threading.Tasks;
using FactoryMind.TrackMe.Domain.Models;

namespace FactoryMind.TrackMe.Business.Repos
{
    public interface IRoomRepository
    {
        Task<List<User>> GetUsersInRoomAsync(int roomId);
        Task<bool> ExistRoomAsync(string roomName);
        Task<bool> IsUserAdminAsync(int userId, string roomName);
        Task<Room> GetRoomAsync(int roomId);
        Task<Room> GetRoomAsync(string roomName);
        Task<Room> CreateRoomAsync(int adminIdn, string name);
        Task<bool> AddUserAsync(int roomId, int id);
        Task<bool> RemoveUserAsync(int roomId, int id);
        Task<bool> UpdateRoomAsync(int id, string newName);
        Task<List<Room>> GetUserRoomsAsync(int userId);
        Task<bool> DeleteRoomAsync(int id);
        Task<bool> DeleteRoomAsync(string name);
    }
}