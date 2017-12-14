using Xunit;
using System;
using FactoryMind.TrackMe.Business.Containers;
using FactoryMind.TrackMe.Business.Services;
using FactoryMind.TrackMe.Business.Repos;
using FactoryMind.TrackMe.Business.Events;
using FactoryMind.TrackMe.Domain.Models;

namespace FactoryMind.TrackMe.Test
{

    public class ServiceFixture : IDisposable
    {
        public PositionService PositionServiceInstance { get; private set; }
        public IPositionReposotory PositionRepoInstance { get; private set; }
        public AuthenticationService AuthenticationServiceInstance { get; private set; }
        public UserService UserService { get; private set; }
        public RoomService RoomServiceInstance { get; private set; }
        public ServiceFixture()
        {
            var container = Container.GetInstance();
            container.Register<AuthenticationService>(Lifestyle.Singleton);
            container.Register<UserService>(Lifestyle.Singleton);
            container.Register<PositionService>(Lifestyle.Singleton);
            container.Register<RoomService>(Lifestyle.Singleton);
            container.Register<IPositionReposotory, DbPositionRepository>(Lifestyle.Singleton);
            container.Register<IRoomRepository, DbRoomRepository>(Lifestyle.Singleton);
            container.Register<IUserRepository, DbUserRepository>(Lifestyle.Singleton);
            container.Register<DataBaseContext>(Lifestyle.Singleton);

            PositionServiceInstance = container.Resolve<PositionService>();
            AuthenticationServiceInstance = container.Resolve<AuthenticationService>();
            UserService = container.Resolve<UserService>();
            RoomServiceInstance = container.Resolve<RoomService>();
            PositionRepoInstance = container.Resolve<IPositionReposotory>();
        }

        void IDisposable.Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}