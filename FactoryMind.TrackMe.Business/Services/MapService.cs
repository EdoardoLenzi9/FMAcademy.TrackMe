using System;
using System.Threading;
using System.Threading.Tasks;

namespace FactoryMind.TrackMe.Business.Services
{
    public class MapService
    {
        public Task<bool> IncludeMapAsync()
        {
            return Task.Run(() =>
            {
                Console.WriteLine("Log: Mappa Inclusa");
                return true;
            });
        }
        
        public Task<bool> SetZoomAsync()
        {
            return Task.Run(() =>
            {
                Console.WriteLine("Log: Zoom Settato");
                return true;
            });
        }
    }
}