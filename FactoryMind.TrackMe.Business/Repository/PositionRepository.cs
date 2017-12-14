using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FactoryMind.TrackMe.Domain.Models;

namespace FactoryMind.TrackMe.Business.Repos
{
    public class PositionRepository : IPositionReposotory
    {
        private static List<Position> Positions = new List<Position>();

        public Task<bool> AddPositionAsync(int id, float x, float y)
        {
            return Task.Run(() =>
            {
                Positions.Add(new Position { X = x, Y = y, UserId = id, Date = DateTime.Now }); //unica eccezione lista nulla
                return true;
            });
        }

        public Task<Position> GetPositionAsync(int id)
        {
            return Task.Run(() =>
            {
                return Positions.FindLast(x => x.UserId == id);
            });
        }

        public Task<Position> GetPositionAsync(int id, DateTime date)
        {
            return Task.Run(() =>
            {
                return Positions.FindLast(x => x.UserId == id && x.Date == date); //puo' ritornare null
            });
        }

        public Task<int> DeletePositionAsync(int id)
        {
            return Task.Run(() =>
            {
                return Positions.RemoveAll(x => x.UserId == id);
            });
        }

        public Task<int> DeletePositionAsync(int id, DateTime date)
        {
            return Task.Run(() =>
            {
                return Positions.RemoveAll(x => x.UserId == id && x.Date == date);
            });
        }

        public Task<int> DeletePositionAsync(int id, float x, float y)
        {
            return Task.Run(() =>
            {
                return Positions.RemoveAll(a => a.UserId == id && a.X == x && a.Y == y);
            });
        }
    }
}