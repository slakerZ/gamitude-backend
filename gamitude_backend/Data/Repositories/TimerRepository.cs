using gamitude_backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using gamitude_backend.Data;

namespace gamitude_backend.Repositories
{
    public interface ITimerRepository
    {
        Task<Timer> getByIdAsync(string id);
        Task<List<Timer>> getByUserIdAsync(string userId);
        Task createAsync(Timer folder);
        Task updateAsync(string id, Timer updateTimer);
        Task deleteByIdAsync(string id);
    }
    public class TimerRepository : ITimerRepository
    {
        private readonly IMongoCollection<Timer> _Timers;


        public TimerRepository(IDatabaseCollections dbCollections)
        {
            _Timers = dbCollections.timers;
        }

        public Task<Timer> getByIdAsync(string id)
        {
            return _Timers.Find<Timer>(Timer => Timer.id == id).FirstOrDefaultAsync();
        }

        public Task<List<Timer>> getByUserIdAsync(string userId)
        {
            return _Timers.Find<Timer>(Timer => Timer.userId == userId).ToListAsync();

        }

        public Task createAsync(Timer Timer)
        {
            return _Timers.InsertOneAsync(Timer);
        }

        public Task updateAsync(string id, Timer newTimer)
        {
            return _Timers.ReplaceOneAsync(Timer => Timer.id == id, newTimer);

        }

        public Task deleteByTimerAsync(Timer TimerIn)
        {
            return _Timers.DeleteOneAsync(Timer => Timer.id == TimerIn.id);
        }

        public System.Threading.Tasks.Task deleteByIdAsync(string id)
        {
            return _Timers.DeleteOneAsync(Timer => Timer.id == id);

        }

    }
}