using System;
using System.Collections.Generic;
using AutoMapper;

namespace FactoryMind.TrackMe.Domain.Models
{
    public static class RoomModelAutoMapper
    {
        public static RoomDto AsDto(this Room room)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Room, RoomDto>());
            var mapper = config.CreateMapper();
            return mapper.Map<RoomDto>(room);
        }

        public static List<RoomDto> AsDto(this List<Room> groupList)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Room, RoomDto>());
            var mapper = config.CreateMapper();
            return mapper.Map<List<Room>, List<RoomDto>>(groupList);
        }
    }
}