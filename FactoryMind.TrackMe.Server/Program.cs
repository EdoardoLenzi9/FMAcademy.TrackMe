using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FactoryMind.TrackMe.Business.Containers;
using FactoryMind.TrackMe.Business.Repos;
using FactoryMind.TrackMe.Business.Services;
using Microsoft.AspNetCore.Hosting;

namespace FactoryMind.TrackMe.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var container = Container.GetInstance();
            /*
            perchè aggiungerli singleton in Program.cs quando vengono aggiunti scoped in Startup.cs?

            container.Register<IPositionReposotory, DbPositionRepository>(Lifestyle.Singleton);
            container.Register<IRoomRepository, DbRoomRepository>(Lifestyle.Singleton);
            container.Register<IUserRepository, DbUserRepository>(Lifestyle.Singleton);
            container.Register<AuthenticationService>(Lifestyle.Singleton);
            container.Register<UserService>(Lifestyle.Singleton);
            container.Register<MapService>(Lifestyle.Singleton);
            container.Register<PositionService>(Lifestyle.Singleton);
            container.Register<RoomService>(Lifestyle.Singleton);
            */
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls("http://+:5001") //di def si mappa su localhost
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
            host.Run();
        }
    }
}
