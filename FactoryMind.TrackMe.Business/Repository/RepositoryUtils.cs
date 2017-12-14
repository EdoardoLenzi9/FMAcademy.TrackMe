using FactoryMind.TrackMe.Business.Containers;
using FactoryMind.TrackMe.Domain.Models;

namespace FactoryMind.TrackMe.Business.Repos
{
    public static class RepositoryUtils
    {
        public static void Init()
        {
            /*registrazioni */
            var container = Container.GetInstance();
            container.Register<IPositionReposotory, DbPositionRepository>(Lifestyle.Singleton);
            container.Register<IRoomRepository, DbRoomRepository>(Lifestyle.Singleton);
            container.Register<IUserRepository, DbUserRepository>(Lifestyle.Singleton);
            container.Register<DataBaseContext>(Lifestyle.Scoped);
        }
    }
}