using FactoryMind.TrackMe.Business.Repos;
using FactoryMind.TrackMe.Business.Containers;
using FactoryMind.TrackMe.Business.Events;

namespace FactoryMind.TrackMe.Business.Services
{
    public static class ServicesUtils
    {
        public static void Init()
        {
            /*registrazioni */
            var container = Container.GetInstance();
            container.Register<AuthenticationService>(Lifestyle.Singleton);
            container.Register<UserService>(Lifestyle.Singleton);
            container.Register<MapService>(Lifestyle.Singleton);
            container.Register<PositionService>(Lifestyle.Singleton);
            container.Register<RoomService>(Lifestyle.Singleton);
            RepositoryUtils.Init();
        }
    }
}