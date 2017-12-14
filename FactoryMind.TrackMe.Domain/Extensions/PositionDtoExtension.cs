using System;
using System.Collections.Generic;
using AutoMapper;

namespace FactoryMind.TrackMe.Domain.Models
{
    public static class PositionModelAutoMapper
    {
        public static PositionDto AsDto(this Position position)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Position, PositionDto>());
            var mapper = config.CreateMapper();
            return mapper.Map<PositionDto>(position);
        }

        public static List<PositionDto> AsDto(this List<Position> groupList)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Position, PositionDto>());
            var mapper = config.CreateMapper();
            return mapper.Map<List<Position>, List<PositionDto>>(groupList);
        }
    }
}