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
        private readonly IMongoCollection<Timer> _timers;


        public TimerService(IDatabaseCollections dbCollections) : base(dbCollections)
        {
            _timers = dbCollections.timers;
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
        new public Task createAsync(Timer timer)
        {
            timer =  checkAndUpdateStopwatch(timer);
            return base.createAsync(timer);
        }
        new public Task updateAsync(string id,Timer timer)
        {
            timer =  checkAndUpdateStopwatch(timer);
            return base.updateAsync(id,timer);
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