using System;

namespace FactoryMind.TrackMe.Domain.Models

{
    public class Position
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public User User { get; set; }
    }
}