using gamitude_backend.Models;
using MongoDB.Driver;
using gamitude_backend.Data;
using gamitude_backend.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace gamitude_backend.Services
{
    public interface ITimerService : ITimerRepository
    {

    }
    public class TimerService : TimerRepository , ITimerService
    {
        private readonly IMongoCollection<Timer> _folders;


        public TimerService(IDatabaseCollections dbCollections) : base(dbCollections)
        {
            _folders = dbCollections.timers;
        }
        new public async Task<Timer> getByIdAsync(string id)
        {
            var timer =  await base.getByIdAsync(id);
            return checkAndUpdateStopwatch(timer);
        }

        new public async Task<List<Timer>> getByUserIdAsync(string userId)
        {
            var timers = await base.getByUserIdAsync(userId);
            return timers.Select(timer => checkAndUpdateStopwatch(timer)).ToList();

        }

        private Timer checkAndUpdateStopwatch(Timer timer)
        {
            if(timer.timerType == TIMER_TYPE.STOPWATCH)
            {
                timer.countDownInfo = new CountDownInfo().initStopWatch();
            }
            return timer;
        }

    }
}