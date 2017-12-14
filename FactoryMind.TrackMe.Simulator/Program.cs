using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FactoryMind.TrackMe.Domain.Models;
using FactoryMind.TrackMe.Simulatore.GpxUtils;
using Newtonsoft.Json;

namespace FactoryMind.TrackMe.Simulator
{
    class Program
    {
        private static Gpx GpxBuilder;
        private static string ConnectionString = "http://172.28.64.175:5001";
        //private static string ConnectionString = "http://localhost:5001";
        private static HttpClient Client = new HttpClient();
        private static HttpRequestMessage RequestMessage;
        private static HttpResponseMessage Answer;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            //MainAsync(counter++).Wait();
        }

        public static async Task MainAsync(int index)
        {
            var userId = new int[10];
            for (int i = 0; i < 10; i++)
            {
                userId[i] = new int();
            }
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    await RegistrationAsync(i);
                }
                catch (Exception)
                {
                    userId[i] = await LoginAsync(i);
                }
                if (userId[i] <= 0)
                {
                    userId[i] = await LoginAsync(i);
                }
                Console.WriteLine($"id: {userId[i]}");
            }
            var c = 0;
            while (true)
            {
                c++;
                for (int i = 1; i < 10; i++)
                {  
                    //c++;
                    var point = GpxUtility.ReadPointAt($"Maps/0{i}.gpx", c);
                    var position = new PositionDto { UserId = userId[i], Y = (float)Math.Round(point.Lat, 6, MidpointRounding.AwayFromZero), X = (float)Math.Round(point.Lon, 6, MidpointRounding.AwayFromZero)};
                    var content = new StringContent(JsonConvert.SerializeObject(position), Encoding.UTF8, "application/json");
                    content.Headers.Add("id", userId[i].ToString());
                    await Client.PostAsync($"{ConnectionString}/position/addposition", content);
                    Console.WriteLine($"Inserita posizione Lat:{position.Y}, Lon:{position.X}, UserId {userId[i]}, from{$"Maps/0{i}.gpx"}, at {c}");
                }
                Thread.Sleep(10000);
            }
        }

        public static async Task<int> LoginAsync(int index)
        {
            RequestMessage = new HttpRequestMessage(HttpMethod.Post, $"{ConnectionString}/authentication/login");
            RequestMessage.Headers.Add("mail", $"mail{index}");
            RequestMessage.Headers.Add("password", $"password{index}");
            Answer = await Client.SendAsync(RequestMessage);
            return ((Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(await Answer.Content.ReadAsStringAsync())).ToObject<UserDto>().Id;
        }

        public static async Task<int> RegistrationAsync(int index)
        {
            RequestMessage = new HttpRequestMessage(HttpMethod.Post, $"{ConnectionString}/user/registration");
            RequestMessage.Headers.Add($"mail", $"mail{index}");
            RequestMessage.Headers.Add($"password", $"password{index}");
            RequestMessage.Headers.Add("gender", "f");
            Answer = await Client.SendAsync(RequestMessage);
            return ((Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(await Answer.Content.ReadAsStringAsync())).ToObject<UserDto>().Id;
        }

        public static void CreateTrack(int userId, int index)
        {
            GpxBuilder = new Gpx();

            var random = new Random();
            var X = 46.117084 + index;
            var Y = 11.104203 + index;
            for (int i = 0; i < 1000; i++)
            {
                var s0 = random.NextDouble() > 0.5;
                var s1 = random.NextDouble() > 0.5;
                if (s0 & s1)
                    X += (float)0.0003;
                if (s0 & !s1)
                    X -= (float)0.0003;
                if (!s0 & s1)
                    Y += (float)0.0003;
                if (!s0 & !s1)
                    Y -= (float)0.0003;
                GpxBuilder.CreatePoint((float)Math.Round(X, 6, MidpointRounding.AwayFromZero), (float)Math.Round(X, 6, MidpointRounding.AwayFromZero));
            }
            GpxBuilder.SaveToLocation("Tracks", $"{index}.gpx");
        }
    }
}
