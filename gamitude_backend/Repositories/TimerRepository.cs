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
    public interface ITimerRepository
    {
        Task<Timer> getByIdAsync(String id);
        Task<List<Timer>> getByUserIdAsync(String userId);
        Task createAsync(Timer folder);
        Task updateAsync(String id, Timer updateTimer);
        Task deleteByIdAsync(String id);
    }
    public class TimerRepository : ITimerRepository
    {
        private readonly IMongoCollection<Timer> _Timers;


        public TimerRepository(IDatabaseCollections dbCollections)
        {
            _Timers = dbCollections.timers;
        }

        public Task<Timer> getByIdAsync(String id)
        {
            return _Timers.Find<Timer>(Timer => Timer.id == id).FirstOrDefaultAsync();
        }

        public Task<List<Timer>> getByUserIdAsync(String userId)
        {
            return _Timers.Find<Timer>(Timer => Timer.userId == userId).ToListAsync();

        }

        public Task createAsync(Timer Timer)
        {
            return _Timers.InsertOneAsync(Timer);
        }

        public Task updateAsync(String id, Timer newTimer)
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