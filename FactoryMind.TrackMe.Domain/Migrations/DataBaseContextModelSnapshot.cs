using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using FactoryMind.TrackMe.Domain.Models;

namespace FactoryMind.TrackMe.Domain.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    partial class DataBaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FactoryMind.TrackMe.Domain.Models.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<int>("UserId");

                    b.Property<float>("X");

                    b.Property<float>("Y");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Position");
                });

            modelBuilder.Entity("FactoryMind.TrackMe.Domain.Models.Room", b =>
                {
                    b.Property<int>("RoomId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AdminId");

                    b.Property<string>("Name");

                    b.HasKey("RoomId");

                    b.ToTable("Room");
                });

            modelBuilder.Entity("FactoryMind.TrackMe.Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("Password");

                    b.Property<int>("UserGender");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("FactoryMind.TrackMe.Domain.Models.UserRoom", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoomId");

                    b.HasKey("UserId", "RoomId");

                    b.HasIndex("RoomId");

                    b.ToTable("UserRoom");
                });

            modelBuilder.Entity("FactoryMind.TrackMe.Domain.Models.Position", b =>
                {
                    b.HasOne("FactoryMind.TrackMe.Domain.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FactoryMind.TrackMe.Domain.Models.UserRoom", b =>
                {
                    b.HasOne("FactoryMind.TrackMe.Domain.Models.Room", "Room")
                        .WithMany("UserRoom")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FactoryMind.TrackMe.Domain.Models.User", "User")
                        .WithMany("UserRoom")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
