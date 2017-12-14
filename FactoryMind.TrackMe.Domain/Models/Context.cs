using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;

namespace FactoryMind.TrackMe.Domain.Models
{
    public class DataBaseContext : DbContext
    {
        public DbSet<Position> Position { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserRoom> UserRoom { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRoom>()
                .HasKey(t => new { t.UserId, t.RoomId });

            modelBuilder.Entity<UserRoom>()
                .HasOne(pt => pt.Room)
                .WithMany(p => p.UserRoom)
                .HasForeignKey(pt => pt.RoomId);

            modelBuilder.Entity<UserRoom>()
                .HasOne(pt => pt.User)
                .WithMany(t => t.UserRoom)
                .HasForeignKey(pt => pt.UserId);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TrackMe;Trusted_Connection=True;MultipleActiveResultSets=true;");
        }


    }
}