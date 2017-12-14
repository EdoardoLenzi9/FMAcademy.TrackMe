using Xunit;
using System;
using FactoryMind.TrackMe.Business.Containers;
using FactoryMind.TrackMe.Business.Services;
using FactoryMind.TrackMe.Business.Exceptions;

namespace FactoryMind.TrackMe.Test
{
    public class RegistrationTest : IClassFixture<ServiceFixture>

    {
        private ServiceFixture _fixture;
        private UserService _userService;
        private AuthenticationService _authenticationService;
        public RegistrationTest(ServiceFixture fix)
        {
            _fixture = fix;
            _userService = _fixture.UserService;
            _authenticationService = _fixture.AuthenticationServiceInstance;
        }

        [Fact]
        public async void Should_LoginSuccessfully_When_UserIsRegistered()
        {
            //Arrange
            var id = await _userService.NewRegistrationAsync("mail", "pw", "m");

            //Act
            var id2 = await _authenticationService.LoginAsync("mail", "pw");

            //Assert
            Assert.Equal(id, id2);
        }

        [Fact]
        public async void NotRegistredUserLoginTest()
        {
            //Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await _authenticationService.LoginAsync("mail2", "pw2");
            });
        }

        [Fact]
        public async void DoubleUserRegistration()
        {
            //Act
            await _userService.NewRegistrationAsync("mail3", "pw3", "m");

            //Assert
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await _userService.NewRegistrationAsync("mail3", "pw3", "m");
            });
        }

        [Fact]
        public async void GenderRegistrationTest()
        {
            //Act & Assert
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                var id2 = await _userService.NewRegistrationAsync("mail", "pw", "?");
            });
        }
    }
}

