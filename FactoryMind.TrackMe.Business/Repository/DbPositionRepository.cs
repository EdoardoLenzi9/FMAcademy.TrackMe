using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FactoryMind.TrackMe.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FactoryMind.TrackMe.Business.Repos
{
    public sealed class DbPositionRepository : IPositionReposotory
    {
        private DataBaseContext db;

        public DbPositionRepository(DataBaseContext dbContext)
        {
            db = dbContext;
        }

        public async Task<bool> AddPositionAsync(int userId, float x, float y)
        {
            db.Position.Add(new Position { UserId = userId, X = x, Y = y });
            if (await db.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<Position> GetPositionAsync(int id)
        {
            var positions = db.Position.Where(x => x.UserId == id);
            if (db.Position.Where(x => x.UserId == id).Count() != 0)
            {
                return await positions.LastAsync();
            }
            return new Position();
        }

        public async Task<Position> GetPositionAsync(int id, DateTime date)
        {
            return await db.Position.Where(x => x.UserId == id && x.Date == date).LastAsync();
        }

        public async Task<int> DeletePositionAsync(int id)
        {
            await db.Position.Where(x => x.UserId == id).ForEachAsync(x => db.Remove(x));
            return await db.SaveChangesAsync();
        }

        public async Task<int> DeletePositionAsync(int id, DateTime date)
        {
            var remove = db.Position.Where(x => x.UserId == id && x.Date == date);
            await remove.ForEachAsync(x => db.Remove(x));
            return await db.SaveChangesAsync();
        }

        public async Task<int> DeletePositionAsync(int id, float x, float y)
        {
            var remove = db.Position.Where(a => a.UserId == id && a.X == x && a.Y == y);
            await remove.ForEachAsync(i => db.Remove(i));
            return await db.SaveChangesAsync();
        }
    }
}