using gamitude_backend.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using gamitude_backend.Settings;
using MongoDB.Driver;
using gamitude_backend.Data;

namespace gamitude_backend.Repositories
{
    public interface IStatsRepository
    {
        Task<Stats> getByIdAsync(String id);
        Task<List<Stats>> getByUserIdAsync(String userId);
        Task createAsync(Stats stats);
        Task updateAsync(String id, Stats updateStats);
        Task deleteByIdAsync(String id);
    }
    public class StatsRepository : IStatsRepository
    {
        private readonly IMongoCollection<Stats> _stats;


        public StatsRepository(IDatabaseCollections dbCollections)
        {
            _stats = dbCollections.stats;
        }

        public Task<Stats> getByIdAsync(String id)
        {
            return _stats.Find<Stats>(Stats => Stats.id == id).FirstOrDefaultAsync();
        }

        public Task<List<Stats>> getByUserIdAsync(String userId)
        {
            return _stats.Find<Stats>(Stats => Stats.userId == userId).ToListAsync();

        }

        public Task createAsync(Stats Stats)
        {
            return _stats.InsertOneAsync(Stats);
        }

        public Task updateAsync(String id, Stats newStats)
        {
            return _stats.ReplaceOneAsync(Stats => Stats.id == id, newStats);

        }

        public Task deleteByStatsAsync(Stats StatsIn)
        {
            return _stats.DeleteOneAsync(Stats => Stats.id == StatsIn.id);
        }

        public System.Threading.Tasks.Task deleteByIdAsync(string id)
        {
            return _stats.DeleteOneAsync(Stats => Stats.id == id);

        }

    }
}