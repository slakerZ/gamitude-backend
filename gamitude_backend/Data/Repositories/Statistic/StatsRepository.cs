using gamitude_backend.Models;
using System.Threading.Tasks;
using MongoDB.Driver;
using gamitude_backend.Data;

namespace gamitude_backend.Repositories
{
    public interface IStatsRepository
    {
        Task<Stats> getByIdAsync(string id);
        Task<Stats> getByUserIdAsync(string userId);
        Task createAsync(Stats stats);
        Task updateAsync(string id, Stats updateStats);
        Task deleteByIdAsync(string id);
    }
    public class StatsRepository : IStatsRepository
    {
        private readonly IMongoCollection<Stats> _stats;


        public StatsRepository(IDatabaseCollections dbCollections)
        {
            _stats = dbCollections.stats;
        }

        public Task<Stats> getByIdAsync(string id)
        {
            return _stats.Find<Stats>(Stats => Stats.id == id).FirstOrDefaultAsync();
        }

        public Task<Stats> getByUserIdAsync(string userId)
        {
            return _stats.Find<Stats>(Stats => Stats.userId == userId).FirstOrDefaultAsync();

        }

        public Task createAsync(Stats Stats)
        {
            return _stats.InsertOneAsync(Stats);
        }

        public Task updateAsync(string id, Stats newStats)
        {
            return _stats.ReplaceOneAsync(Stats => Stats.id == id, newStats,new ReplaceOptions{IsUpsert=true});

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