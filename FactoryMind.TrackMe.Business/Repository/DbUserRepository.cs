using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FactoryMind.TrackMe.Domain.Models;
using Microsoft.EntityFrameworkCore;
using static FactoryMind.TrackMe.Domain.Models.User;

namespace FactoryMind.TrackMe.Business.Repos
{
    public sealed class DbUserRepository : IUserRepository
    {
        private DataBaseContext db;

        public DbUserRepository(DataBaseContext dbContext)
        {
            db = dbContext;
        }

        public async Task<User> CreateUserAsync(string email, string password, User.Gender gender)
        {
            db.User.Add(new User { Email = email, Password = password, UserGender = gender });
            await db.SaveChangesAsync();
            return await db.User.SingleAsync(x => x.Email == email);
        }

        public async Task<bool> DeleteUserAsync(string email)
        {
            var user = await db.User.SingleAsync(u => u.Email == email);
            db.Remove(user);
            if (await db.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await db.User.SingleAsync(u => u.Id == id);
            db.Remove(user);
            if (await db.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<User> GetUserAsync(string email)
        {
            return await db.User.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserAsync(int id)
        {
            return await db.User.SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<User>> GetUsersAsync(Gender gender)
        {
            var userList = await db.User.Where(user => user.UserGender == gender).ToListAsync();
            return userList.Where(user => user.UserGender == gender).ToList();
        }

        public async Task<bool> IsUserInDatabaseAsync(int userId)
        {
            if (await db.User.SingleOrDefaultAsync(u => u.Id == userId) == null)
            {
                return false;
            }
            return true;
        }

        public Task<List<User>> GetAllUsersAsync()
        {
            return Task.Run(() =>
            {
                return db.User.ToListAsync();
            });
        }
    }
}