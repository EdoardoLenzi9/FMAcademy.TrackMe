using System;
using FactoryMind.TrackMe.Business.Services;
using System.Threading.Tasks;
using FactoryMind.TrackMe.Business.Containers;
using FactoryMind.TrackMe.Business.Repos;
using FactoryMind.TrackMe.Business.Events;
using System.Collections.Generic;
using FactoryMind.TrackMe.Business.Exceptions;
using System.Text;
using FactoryMind.TrackMe.Business.Utility;
using Newtonsoft.Json;
using System.Net.Http;
using FactoryMind.TrackMe.Domain.Models;
using System.Linq;
using System.Threading;

namespace FactoryMind.TrackMe.UiClient
{

    public class ConsoleApplication
    {
        private static UserDto CurrentUser = null;
        private static RoomDto CurrentRoom = null;
        private static PositionService PositionServiceInstance;
        private static AuthenticationService AuthorizationServiceInstance;
        private static MapService MapServiceInstance;
        private static UserService UserServiceInstance;
        private static RoomService RoomServiceInstance;
        private static Container ContainerInstance;
        private static PositionEngine PositionEngineInstance;
        //private static string ConnectionString = "http://172.28.64.175:5001";
        private static string ConnectionString = "http://localhost:5001";
        private static HttpClient Client = new HttpClient();
        private static HttpRequestMessage RequestMessage;
        private static HttpResponseMessage Answer;
        private static bool ServerOnline= true;

        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Clear();
            //ServicesUtils.Init();
            //Resolve();
            MainAsync().Wait();
        }

        public static void Resolve()
        {
            ContainerInstance = Container.GetInstance();
            PositionServiceInstance = ContainerInstance.Resolve<PositionService>();
            AuthorizationServiceInstance = ContainerInstance.Resolve<AuthenticationService>();
            MapServiceInstance = ContainerInstance.Resolve<MapService>();
            UserServiceInstance = ContainerInstance.Resolve<UserService>();
            RoomServiceInstance = ContainerInstance.Resolve<RoomService>();
        }

        public static async Task MainAsync()
        {
            while (true)
            {
                if (await Utility.IsServerOnlineAsync(ConnectionString))
                {
                    System.Console.WriteLine("Server Online");
                    try
                    {
                        await LoginPageAsync();
                        await MainPageAsync();
                    }
                    catch (Exception)
                    {
                        System.Console.WriteLine("client crash :(");
                        Console.ReadLine();
                    }
                }
                else
                {
                    Console.WriteLine("Server Offline");
                    Console.ReadLine();
                }

            }
        }

        public async static Task LoginPageAsync()
        {
            var flag = false;
            while (!flag)
            {
                Console.Clear();
                Console.WriteLine("\n%%%%%%%%%%%%%%%%%%%%%%");
                Console.WriteLine("%%   Initial Page   %%");
                Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%\n");
                Console.WriteLine("Insert: l (Login), r (Registration) or q (Quit)");
                var option = Console.ReadLine();
                switch (option)
                {
                    case "l":
                        Console.WriteLine("\n%%%%%%%%%%%%%%%%%%%%%%");
                        Console.WriteLine("%%%   Login Page   %%%");
                        Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%\n");
                        Console.WriteLine("Insert: <Mail> [Enter] <Password> [Enter]");

                        // ottengo UserId
                        RequestMessage = new HttpRequestMessage(HttpMethod.Post, $"{ConnectionString}/api/1/authentication/login");
                        RequestMessage.Headers.Add("mail", Console.ReadLine());
                        RequestMessage.Headers.Add("password", Console.ReadLine());
                        Answer = await Client.SendAsync(RequestMessage);
                        if (Answer.StatusCode.ToString() == "BadRequest") // perchè bad request?
                        {
                            System.Console.WriteLine("Errore login, utente inesistente o password errata");
                            System.Console.ReadLine();
                            continue;
                        }
                        CurrentUser = ((Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(await Answer.Content.ReadAsStringAsync())).ToObject<UserDto>();

                        // entro nella room di default
                        RequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{ConnectionString}/api/1/room/opendefaultroom");
                        RequestMessage.Headers.Add("id", CurrentUser.Id.ToString());
                        Answer = await Client.SendAsync(RequestMessage);
                        CurrentRoom = ((Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(await Answer.Content.ReadAsStringAsync())).ToObject<RoomDto>();
                        System.Console.WriteLine($"id room da opendefaultroom = {CurrentRoom.RoomId} \nnome = {CurrentRoom.Name}");

                        if (CurrentUser != null) flag = true;
                        break;
                    case "r":
                        Console.WriteLine("\n%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
                        Console.WriteLine("%%%   Registration Page   %%%");
                        Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%\n");
                        Console.WriteLine("Insert: <Mail> [Enter] <Password> [Enter] <Gender> [m/f/sm] [Enter]");

                        // richiedo registrazione
                        RequestMessage = new HttpRequestMessage(HttpMethod.Post, $"{ConnectionString}/api/1/user/registration");
                        RequestMessage.Headers.Add("mail", Console.ReadLine());
                        RequestMessage.Headers.Add("password", Console.ReadLine());
                        RequestMessage.Headers.Add("gender", Console.ReadLine());
                        Answer = await Client.SendAsync(RequestMessage);

                        if (Answer.StatusCode.ToString() == "InternalServerError")
                        {
                            System.Console.WriteLine("Errore registrazione, utente già presente in db");
                            System.Console.ReadLine();
                            continue;
                        }

                        CurrentUser = ((Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(await Answer.Content.ReadAsStringAsync())).ToObject<UserDto>();
                        System.Console.WriteLine($"id da registrazione = {CurrentUser.Id} \nemail = {CurrentUser.Email}");

                        // entro nella room di default
                        RequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{ConnectionString}/api/1/room/opendefaultroom");
                        RequestMessage.Headers.Add("id", CurrentUser.Id.ToString());
                        Answer = await Client.SendAsync(RequestMessage);
                        CurrentRoom = ((Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(await Answer.Content.ReadAsStringAsync())).ToObject<RoomDto>();
                        System.Console.WriteLine($"id room da opendefaultroom = {CurrentRoom.RoomId} \nnome = {CurrentRoom.Name}");
                        if (CurrentUser != null) flag = true;
                        break;
                    case "q":
                        System.Environment.Exit(0);
                        break;
                }
            }
        }

        public static async void SetPositionAsync(Object send, Business.Events.EventArgs ea) // funzione chiamata da engine per inserire la propria posizione nel server
        {
            var position = new PositionDto { UserId = CurrentUser.Id, X = ea.X, Y = ea.Y };
            var content = new StringContent(JsonConvert.SerializeObject(position), Encoding.UTF8, "application/json");
            content.Headers.Add("id", CurrentUser.Id.ToString());
            var response =  new HttpResponseMessage();
            try
            {
                response = await Client.PostAsync($"{ConnectionString}/api/1/position/addposition", content);
                var result = await response.Content.ReadAsStringAsync();
                if(!ServerOnline)
                {
                    System.Console.WriteLine("Connessione con il server ristabilita");
                    ServerOnline = true;
                }
            }
            catch (Exception)
            {
                System.Console.WriteLine("Server Offline");
                ServerOnline = false;
            }
        }

        public async static Task MainPageAsync()
        {
            Console.Clear();
            Console.WriteLine("\n%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
            Console.WriteLine("%%%                       Pagina Principale                      %%%");
            Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%\n");
            Console.WriteLine("\n%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
            Console.WriteLine("%%%                             Mappa                            %%%");
            Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%\n");

            // start engine per set position al server
            PositionEngineInstance = new PositionEngine();
            PositionEngineInstance.PositionEvent += SetPositionAsync;

            // ottenimento posizioni altri utenti nella mia room
            RequestMessage = new HttpRequestMessage(HttpMethod.Post, $"{ConnectionString}/api/1/position/getpoints");
            RequestMessage.Headers.Add("id", CurrentUser.Id.ToString());
            RequestMessage.Content = new StringContent(JsonConvert.SerializeObject(CurrentRoom), Encoding.UTF8, "application/json"); // aggiungo json della room
            Answer = await Client.SendAsync(RequestMessage);
            var positionList = ((Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(await Answer?.Content.ReadAsStringAsync())).ToObject<List<PositionDto>>();
            positionList?.ForEach(pos => System.Console.WriteLine($"posizione: id user=[{pos.UserId}], x={pos.X}, y={pos.Y}"));

            var flag = false;
            while (!flag)
            {
                Console.WriteLine("Insert: s (Show Rooms), m (Manage Rooms), b(Back to Main Menu) or l (Logout)");
                var option = Console.ReadLine();
                Console.Clear();
                switch (option)
                {
                    case "s":
                        RequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{ConnectionString}/api/1/room/showroom");
                        RequestMessage.Headers.Add("id", CurrentUser.Id.ToString());
                        Answer = await Client.SendAsync(RequestMessage);
                        var roomList = ((Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(await Answer.Content.ReadAsStringAsync())).ToObject<List<string>>();
                        roomList.ForEach(r => Console.WriteLine(r));
                        break;
                    case "m":
                        await RoomPageAsync();
                        break;
                    case "b":
                        await MainPageAsync();
                        break;
                    case "l":
                        flag = true;
                        CurrentUser = null;
                        CurrentRoom = null; // reset variabili locali
                        PositionEngineInstance.PositionEvent -= SetPositionAsync; // rimozione evento dall'engine
                        break;
                }
            }
        }

        public async static Task ModRoomPageAsync()
        {
            Console.Clear();
            Console.WriteLine("\n%%%%%%%%%%%%%%%%%%%%%%%%%");
            Console.WriteLine("%%     ModRoom Page    %%");
            Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%\n");
            var flag = false;
            while (!flag)
            {
                Console.WriteLine("Insert: a (Add User to Room), r (Remove User from Room), q (Quit)");
                var option = Console.ReadLine();
                switch (option)
                {
                    case "a":
                        // inserimento utente in room
                        Console.WriteLine("Insert: <Person Name> [ENTER] <Room Name> [ENTER]");
                        RequestMessage = new HttpRequestMessage(HttpMethod.Put,
                            $"{ConnectionString}/api/1/room/addperson/{Console.ReadLine()}/toroom/{Console.ReadLine()}");
                        RequestMessage.Headers.Add("id", CurrentUser.Id.ToString());
                        Answer = await Client.SendAsync(RequestMessage);
                        break;
                    case "r":
                        // rimozione utente dalla room
                        Console.WriteLine("Insert: <User Name> [ENTER] <Room Name> [ENTER]");
                        RequestMessage = new HttpRequestMessage(HttpMethod.Put,
                            $"{ConnectionString}/api/1/room/removeperson/{Console.ReadLine()}/fromroom/{Console.ReadLine()}");
                        RequestMessage.Headers.Add("id", CurrentUser.Id.ToString());
                        Answer = await Client.SendAsync(RequestMessage);
                        System.Console.WriteLine(await Answer.Content.ReadAsStringAsync());
                        break;
                    case "q":
                        flag = true;
                        break;
                }
            }
        }

        public async static Task RoomPageAsync()
        {
            Console.Clear();
            Console.WriteLine("\n%%%%%%%%%%%%%%%%%%%%%%");
            Console.WriteLine("%%     Room Page    %%");
            Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%\n");
            var flag = false;
            while (!flag)
            {
                Console.WriteLine("Insert: a (Add Room), d (Delete Room), m (Mod Room) or q (Quit)");
                var option = Console.ReadLine();
                switch (option)
                {
                    case "a":
                        // creazione room
                        Console.WriteLine("Insert: <Room Name> [ENTER]");
                        RequestMessage = new HttpRequestMessage(HttpMethod.Post, $"{ConnectionString}/api/1/room/createroom/{Console.ReadLine()}");
                        RequestMessage.Headers.Add("id", CurrentUser.Id.ToString());
                        Answer = await Client.SendAsync(RequestMessage);
                        CurrentRoom = ((Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(await Answer.Content.ReadAsStringAsync())).ToObject<RoomDto>();
                        System.Console.WriteLine("room creata");
                        break;
                    case "d":
                        // eliminazione room
                        Console.WriteLine("Insert: <Room Name> [ENTER]");
                        RequestMessage = new HttpRequestMessage(HttpMethod.Delete, $"{ConnectionString}/api/1/room/deleteroom/{Console.ReadLine()}");
                        RequestMessage.Headers.Add("id", CurrentUser.Id.ToString());
                        Answer = await Client.SendAsync(RequestMessage);
                        break;
                    case "m":
                        await ModRoomPageAsync();
                        break;
                    case "q":
                        flag = true;
                        break;
                }
            }
        }
    }
}
