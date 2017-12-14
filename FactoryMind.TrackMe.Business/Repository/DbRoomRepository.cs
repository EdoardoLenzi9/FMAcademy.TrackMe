using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FactoryMind.TrackMe.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FactoryMind.TrackMe.Business.Repos
{
    public sealed class DbRoomRepository : IRoomRepository
    {
        private DataBaseContext db;

        public DbRoomRepository(DataBaseContext dbContext)
        {
            db = dbContext;
        }

        public async Task<bool> ExistRoomAsync(string roomName)
        {
            return await db.Room.AnyAsync(x => x.Name == roomName);
        }

        public async Task<Room> GetRoomAsync(int userId, int roomId)
        {
            return await db.Room.SingleOrDefaultAsync(x => x.AdminId == userId && x.RoomId == roomId);
        }

        public async Task<bool> AddUserAsync(int roomId, int userId)
        {
            var room = await db.Room.SingleAsync(x => x.RoomId == roomId);
            room.UserRoom.Add(new UserRoom { UserId = userId, RoomId = roomId });
            if (await db.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<Room> CreateRoomAsync(int adminId, string name)
        {
            var room = new Room { AdminId = adminId, Name = name, UserRoom = new List<UserRoom>() };
            room.UserRoom.Add(new UserRoom { UserId = adminId, RoomId = room.RoomId });
            await db.Room.AddAsync(room);
            await db.SaveChangesAsync();
            return room;
        }

        public async Task<bool> DeleteRoomAsync(int id)
        {
            var room = db.Room.Single(x => x.RoomId == id);
            db.Remove(room);
            if (await db.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteRoomAsync(string name)
        {
            var room = db.Room.Where(x => x.Name == name);
            db.Remove(room);
            if (await db.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<List<Room>> GetUserRoomsAsync(int userId)
        {
            return await db.UserRoom.Include(Uroom => Uroom.Room)
                                    .Where(x => x.UserId == userId)
                                    .Select(x => x.Room)
                                    .ToListAsync();
        }

        public async Task<bool> RemoveUserAsync(int roomId, int id)
        {
            var userRoom = await db.UserRoom.SingleOrDefaultAsync(x => x.UserId == id && x.RoomId == roomId);
            var room = await db.Room.SingleAsync(x => x.RoomId == roomId);
            room.UserRoom.Remove(userRoom);
            if (await db.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateRoomAsync(int id, string newName)
        {
            var room = db.Room.Single(x => x.RoomId == id);
            room.Name = newName;
            if (await db.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<Room> GetRoomAsync(int roomId)
        {
            return await db.Room.SingleOrDefaultAsync(x => x.RoomId == roomId);
        }

        public async Task<Room> GetRoomAsync(string roomName)
        {
            return await db.Room.SingleOrDefaultAsync(x => x.Name == roomName);
        }

        public async Task<List<User>> GetUsersInRoomAsync(int roomId)
        {
            var room = await db.Room.SingleOrDefaultAsync(r => r.RoomId == roomId);
            return await db.UserRoom.Include(Uroom => Uroom.User)
                                    .Where(x => x.RoomId == roomId)
                                    .Select(x => x.User)
                                    .ToListAsync();
        }

        public async Task<bool> IsUserAdminAsync(int userId, string roomName)
        {
            var room = await db.Room.SingleOrDefaultAsync(x => x.Name == roomName);
            if(room.AdminId == userId)
            {
                return true;
            }
            return false;
        }
    }
}