using System.Threading.Tasks;
using FactoryMind.TrackMe.Business.Containers;
using FactoryMind.TrackMe.Domain.Models;
using FactoryMind.TrackMe.Business.Repos;
using FactoryMind.TrackMe.Business.Exceptions;
using System;
using System.Collections.Generic;

namespace FactoryMind.TrackMe.Business.Services
{
    public class UserService
    {
        private IUserRepository _uRepo;

        public UserService(IUserRepository us)
        {
            _uRepo = us;
        }

        public async Task<User> NewRegistrationAsync(string mail, string password, string gender)
        {
            if (String.IsNullOrEmpty(mail) || String.IsNullOrEmpty(password) || String.IsNullOrEmpty(gender))
            {
                throw new ParameterException("errore parametri in [NewRegistrationAsync]");
            }
            var userIdInDb = await _uRepo.GetUserAsync(mail);
            if (userIdInDb != null)
            {
                throw new GeneralException("Utente già presente in db [NewRegistrationAsync]");
            }
            User.Gender genderEnum;
            switch (gender)
            {
                case "m":
                    genderEnum = User.Gender.Male;
                    break;
                case "f":
                    genderEnum = User.Gender.Female;
                    break;
                case "sm":
                    genderEnum = User.Gender.Female;
                    break;
                default:
                    throw new RepositoryException("genere non riconosciuto [NewRegistrationAsync]");
            }
            var user = await _uRepo.CreateUserAsync(mail, password, genderEnum);
            if (user == null)
            {
                throw new RepositoryException("Impossibile creare utente in [NewRegistrationAsync]");
            }
            return user;
        }

        public async Task<List<User>> GetUserList(int userId)
        {
            if (userId<1)
            {
                throw new ParameterException("errore parametri in [GetUserList]");
            }
            var users = await _uRepo.GetAllUsersAsync();
            if(users == null || users.Count == 0)
            {
                throw new RepositoryException("errore lista utenti in [GetUserList]");
            }
            return users;
        }
    }
}