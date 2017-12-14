using System.Collections.Generic;

namespace FactoryMind.TrackMe.Domain.Models
{
    public class User
    {
        public enum Gender
        {
            Male,
            Female,
            SheMale
        };

        public string Email { get; set; }
        public string Password { get; set; }
        public int Id { get; set; }
        public Gender UserGender { get; set; }
        public List<UserRoom> UserRoom { get; set; }
    }
}