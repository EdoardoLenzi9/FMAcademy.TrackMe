using System;
using System.Threading.Tasks;
using FactoryMind.TrackMe.Business.Containers;
using FactoryMind.TrackMe.Business.Exceptions;
using FactoryMind.TrackMe.Business.Repos;
using FactoryMind.TrackMe.Domain.Models;

namespace FactoryMind.TrackMe.Business.Services
{
    public class AuthenticationService
    {
        private IUserRepository userRepoInstance;

        public AuthenticationService(IUserRepository userRepo)
        {
            userRepoInstance = userRepo;
        }

        public async Task<bool> isUserRegistredAsync(int id)
        {
            var res = await userRepoInstance.GetUserAsync(id);
            return (res != null);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await userRepoInstance.GetUserAsync(id);
        }

        public async Task<User> LoginAsync(string mail, string password)
        {
            if (String.IsNullOrEmpty(mail) || String.IsNullOrEmpty(password))
            {
                throw new ParameterException("errore parametri in [LoginAsync]");
            }
            var recivedUser = await userRepoInstance.GetUserAsync(mail);
            if (recivedUser.Password == password)
            {
                var user = await userRepoInstance.GetUserAsync(mail);
                if (user == null)
                {
                    throw new NotFoundException("Utente non trovato in [LoginAsync]");
                }
                return user;
            }
            throw new GeneralException("Password errata [LoginAsync]");
        }
    }
}