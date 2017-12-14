using System;
using System.Linq;
using FactoryMind.TrackMe.Domain.Models;

namespace FactoryMind.TrackMe.Business.Utility
{
    public class DbReset
    {
        public DbReset()
        {
            using (var db = new DataBaseContext())
            {
                var rooms = db.Room.ToList();
                var users = db.User.ToList();
                var positions = db.Position.ToList();
                var userRooms = db.UserRoom.ToList();
                rooms.ForEach(r=>db.Remove(r));
                users.ForEach(u=>db.Remove(u));
                positions.ForEach(pos=>db.Remove(pos));
                userRooms.ForEach(u=>db.Remove(u));
                db.SaveChanges();
                Console.WriteLine("db reset");
            }
        }
    }
}