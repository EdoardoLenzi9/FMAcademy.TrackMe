using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FactoryMind.TrackMe.Domain.Models;
using static FactoryMind.TrackMe.Domain.Models.User;

namespace FactoryMind.TrackMe.Business.Repos
{
    public sealed class UserRepository : IUserRepository
    {
        private List<User> _users = new List<User>();

        public Task<User> CreateUserAsync(string email, string password, Gender gender)
        {
            return Task.Run(() =>
            {
                var lastRegisteredUserId = 1;
                if (_users.Count != 0)
                {
                    lastRegisteredUserId = _users.Select(u => u.Id).Max() + 1;
                }
                var user = new User { Email = email, Password = password, UserGender = gender, Id = lastRegisteredUserId };
                _users.Add(user);
                //Thread.Sleep(400); // simulazione ritardo server
                return user;
            });
        }

        public Task<bool> DeleteUserAsync(string email)
        {
            return Task.Run(() =>
            {
                _users.Remove(_users.Single(u => u.Email == email));
                //Thread.Sleep(400); // simulazione ritardo server
                return true;
            });
        }

        public Task<bool> DeleteUserAsync(int id)
        {
            return Task.Run(() =>
            {
                _users.Remove(_users.Single(u => u.Id == id));
                //Thread.Sleep(400); // simulazione ritardo server
                return true;
            });
        }

        public Task<User> GetUserAsync(string email)
        {
            return Task.Run(() =>
            {
                //Thread.Sleep(400); // simulazione ritardo server
                var user = _users.SingleOrDefault(u => u.Email == email);
                if (user == null)
                {
                    return null;
                }
                return user;
            });
        }

        public Task<List<User>> GetUsersAsync(Gender gender)
        {
            return Task.Run(() =>
            {
                return _users.Where(u => u.UserGender == gender).ToList();
            });
        }

        public Task<User> GetUserAsync(int id)
        {
            return Task.Run(() =>
            {
                return _users.SingleOrDefault(u => u.Id == id);
            });
        }

        public Task<bool> IsUserInDatabaseAsync(int userId)
        {
            return Task.Run(() =>
            {
                var users = _users.Where(user => user.Id == userId);
                return users.Any(user => user.Id == userId);
            });
        }

        public Task<List<User>> GetAllUsersAsync()
        {
            return Task.Run(() =>
            {
                return _users;
            });
        }
    }
}