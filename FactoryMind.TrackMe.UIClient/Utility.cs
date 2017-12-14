using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FactoryMind.TrackMe.UiClient
{
    public static class Utility
    {
        private static HttpClient Client = new HttpClient();
        public static async Task<bool> IsServerOnlineAsync(string connectionString)
        {
            try
            {
                System.Console.WriteLine("Connessione in corso...");
                var RequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{connectionString}/api/1/utils/ping");
                var Answer = await Client.SendAsync(RequestMessage);
                System.Console.WriteLine(await Answer.Content.ReadAsStringAsync());
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}