using Xunit;
using System;
using FactoryMind.TrackMe.Business.Containers;
using FactoryMind.TrackMe.Business.Services;
using FactoryMind.TrackMe.Business.Exceptions;

namespace FactoryMind.TrackMe.Test
{
    public class RoomTest : IClassFixture<ServiceFixture>
    {
        ServiceFixture _fixture;
        RoomService _roomService;
        UserService _userService;
        public RoomTest(ServiceFixture fix)
        {
            _fixture = fix;
            _roomService = _fixture.RoomServiceInstance;
            _userService = _fixture.UserService;
        }

        [Fact]
        public async void DoubleCreateRoomTest()
        {
            //Arrange
            var user = await _userService.NewRegistrationAsync("mail5", "pw5", "f");

            //Act
            await _roomService.CreateRoomAsync(user.Id, "ciao");

            //Assert
            await Assert.ThrowsAsync<GeneralException>(async () =>
            {
                await _roomService.CreateRoomAsync(user.Id, "ciao");
            });
        }
        [Fact]
        public async void UserFantomCreateRoomTest()
        {
            //Act & Assert
            await Assert.ThrowsAsync<GeneralException>(async () =>
            {
                await _roomService.CreateRoomAsync(10000, "ciao2");
            });
        }
        [Fact]
        public async void DoubleDeleteRoomTest()
        {
            //Arrange
            var user = await _userService.NewRegistrationAsync("mail5", "pw5", "f");
            var room = await _roomService.CreateRoomAsync(user.Id, "ciao1");

            //Act
            await _roomService.DeleteRoomAsync(user.Id, "ciao1");

            //Assert
            await Assert.ThrowsAsync<GeneralException>(async () =>
            {
                await _roomService.DeleteRoomAsync(user.Id, "ciao1");
            });
        }
        [Fact]
        public async void DoubleRemoveRoomTest()
        {
            //Arrange
            var admin = await _userService.NewRegistrationAsync("mail6", "pw6", "f");
            var idGuest = await _userService.NewRegistrationAsync("mail7", "pw7", "f");
            var roomId = await _roomService.CreateRoomAsync(admin.Id, "ciao3");
            await _roomService.AddPersonToRoomAsync(admin.Id, "ciao3", "mail7");

            //Act
            await _roomService.RemovePersonFromRoomAsync(admin.Id, "ciao3", "mail7");

            //Assert
            await Assert.ThrowsAsync<GeneralException>(async () =>
            {
                await _roomService.RemovePersonFromRoomAsync(admin.Id, "ciao3", "mail7");
            });
        }
        [Fact]
        public async void DoubleAddRoomTest()
        {
            //Arrange
            var rService = _fixture.UserService;
            var admin = await rService.NewRegistrationAsync("mail8", "pw8", "m");
            var idGuest = await rService.NewRegistrationAsync("mail9", "pw9", "f");
            var roomService = _fixture.RoomServiceInstance;
            var roomId = roomService.CreateRoomAsync(admin.Id, "ciao4");

            //Act
            await roomService.AddPersonToRoomAsync(admin.Id, "ciao4", "mail9");

            //Assert
            await Assert.ThrowsAsync<GeneralException>(async () =>
                {
                    await roomService.AddPersonToRoomAsync(admin.Id, "ciao4", "mail9");
                });
        }
    }
}

