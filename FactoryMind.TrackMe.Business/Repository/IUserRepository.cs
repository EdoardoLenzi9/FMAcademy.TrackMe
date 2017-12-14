using System.Collections.Generic;
using System.Threading.Tasks;
using FactoryMind.TrackMe.Domain.Models;
using static FactoryMind.TrackMe.Domain.Models.User;

namespace FactoryMind.TrackMe.Business.Repos
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(string email, string password, Gender gender);
        Task<User> GetUserAsync(int id);  
        Task<User> GetUserAsync(string email);  
        Task<List<User>> GetUsersAsync(Gender gender);  
        Task<List<User>> GetAllUsersAsync();  
        Task<bool> DeleteUserAsync(string email);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> IsUserInDatabaseAsync(int userId);
    }
}