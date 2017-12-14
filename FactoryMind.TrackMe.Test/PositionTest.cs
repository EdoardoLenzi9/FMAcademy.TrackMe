using Xunit;
using System;
using FactoryMind.TrackMe.Business.Containers;
using FactoryMind.TrackMe.Business.Services;
using FactoryMind.TrackMe.Business.Repos;

namespace FactoryMind.TrackMe.Test
{
    public class PositionTest : IClassFixture<ServiceFixture>
    {
        ServiceFixture _fixture;
        PositionService _positionService;
        UserService _userService;
        IPositionReposotory _positionRepo;
        RoomService _roomService;
        public PositionTest(ServiceFixture fix)
        {
            _fixture = fix;
            _positionService = _fixture.PositionServiceInstance;
            _userService = _fixture.UserService;
            _roomService = _fixture.RoomServiceInstance;
            _positionRepo = _fixture.PositionRepoInstance;
        }

        [Fact]
        public async void AddPositionTest()
        {
            //Arrange
            var user = await _userService.NewRegistrationAsync("mail", "pw", "m");
            var roomId = await _roomService.CreateRoomAsync(user.Id, "room1");
            await _positionService.AddPositionAsync(user.Id, 10, 20);

            //Act
            var position = await _positionRepo.GetPositionAsync(user.Id);

            //Assert
            Assert.Equal(10, position.X);
            Assert.Equal(20, position.Y);
        }
    }
}

