using System;
using System.Threading.Tasks;
using FactoryMind.TrackMe.Domain.Models;

namespace FactoryMind.TrackMe.Business.Repos
{
    public interface IPositionReposotory
    {
        Task<bool> AddPositionAsync(int id, float x, float y);
        Task<Position> GetPositionAsync(int id);
        Task<Position> GetPositionAsync(int id, DateTime date);
        Task<int> DeletePositionAsync(int id);
        Task<int> DeletePositionAsync(int id, DateTime date);
        Task<int> DeletePositionAsync(int id, float x, float y);
    }
}