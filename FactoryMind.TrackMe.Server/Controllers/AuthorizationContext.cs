using System;
using FactoryMind.TrackMe.Domain.Models;

namespace FactoryMind.TrackMe.Server.Controllers
{
    public class AuthorizationContext : IAuthorizationContext
    {
        public User User{get;set;}
    }
}