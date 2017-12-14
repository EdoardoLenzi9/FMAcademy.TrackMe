using System;
using System.Collections.Generic;
using AutoMapper;

namespace FactoryMind.TrackMe.Domain.Models
{
    public static class UserModelAutoMapper
    {
        public static UserDto AsDto(this User room)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDto>());
            var mapper = config.CreateMapper();
            return mapper.Map<UserDto>(room);
        }

        public static List<UserDto> AsDto(this List<User> groupList)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDto>());
            var mapper = config.CreateMapper();
            return mapper.Map<List<User>, List<UserDto>>(groupList);
        }
    }
}