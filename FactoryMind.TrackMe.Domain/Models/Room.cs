
using System.Collections.Generic;

namespace FactoryMind.TrackMe.Domain.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public int AdminId { get; set; }
        public string Name { get; set; }
        public List<int> UsersId = new List<int>(); 
        public List<UserRoom> UserRoom { get; set; } = new List<UserRoom>();
    }
}