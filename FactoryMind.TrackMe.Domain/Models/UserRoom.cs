namespace FactoryMind.TrackMe.Domain.Models
{
    public class UserRoom
    {
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public Room Room { get; set; }
        public User User { get; set; }
    }
}