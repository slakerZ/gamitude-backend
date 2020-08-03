using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using gamitude_backend.Dto.TimeSpend;
using gamitude_backend.Dto;
using gamitude_backend.Models;
using gamitude_backend.Services;

namespace gamitude_backend.BusinessLogic
{//TODO talk about time zones ???
    public interface ITimeManager
    {
        Task<GetTimeSpend> manageTime(TimeSpend timeSpend);
    }

    public class TimeManager : ITimeManager
    {
        /// <summary>
        /// This class is responisible for parsing time spend and splitting into energy and stats
        /// </summary>

        /// <remarks>
        /// Split this module to energy Manager and Stats Manager
        /// </remarks>
        private readonly ILogger<TimeManager> _logger;
        private readonly IMapper _mapper;
        private readonly ITimeSpendService _timeSpendsService;
        private readonly IDailyStatsService _dailyStatsService;
        private readonly IDailyEnergyService _dailyEnergyService;
        private readonly IRankManager _rankManager;
        //TODO change to class atributtes
            // private Dictionary<STATS, int> wages;
            // private TimeSpend timeSpend;
            // private int wagesSum;
        public TimeManager(ILogger<TimeManager> logger, IMapper mapper, ITimeSpendService timeSpendsService
                            , IDailyStatsService dailyStatsService, IDailyEnergyService dailyEnergyService
                            , IRankManager rankManager)
        {
            _logger = logger;
            _mapper = mapper;
            _timeSpendsService = timeSpendsService;
            _dailyStatsService = dailyStatsService;
            _dailyEnergyService = dailyEnergyService;
            _rankManager = rankManager;
        }
        public async Task<GetTimeSpend> manageTime(TimeSpend timeSpend)
        {
            try
            {
                Dictionary<STATS, int> wages = timeSpend.getWages();
                List<Task> managersTasks = new List<Task>();
                managersTasks.Add(Task.Run(() => manageEnergyAsync(wages, timeSpend)));
                managersTasks.Add(Task.Run(() => manageStatsAsync(wages, timeSpend)));
                managersTasks.Add(Task.Run(() => _timeSpendsService.CreateAsync(timeSpend)));
                await Task.WhenAll(managersTasks);
                await _rankManager.manageRank(timeSpend.UserId);
                return _mapper.Map<GetTimeSpend>(timeSpend);
            }
            catch (Exception e)
            {
                _logger.LogError("Error cached in TimeManager manageTime {error}", e);
                throw e;
            }
        }
        private async Task manageEnergyAsync(Dictionary<STATS, int> wages, TimeSpend timeSpend)
        {
            DailyEnergy dailyEnergy = calculateEnergy(wages, timeSpend);
            dailyEnergy.UserId = timeSpend.UserId;
            dailyEnergy.Date = DateTime.UtcNow.Date;
            _logger.LogInformation(dailyEnergy.ToString());
            await _dailyEnergyService.CreateOrAddAsync(dailyEnergy);
        }
        private async Task manageStatsAsync(Dictionary<STATS, int> wages, TimeSpend timeSpend)
        {
            DailyStats dailyStats = calculateStats(wages, timeSpend);
            dailyStats.UserId = timeSpend.UserId;
            dailyStats.Date = DateTime.UtcNow.Date;
            await _dailyStatsService.CreateOrAddAsync(dailyStats);
        }
        private DailyEnergy calculateEnergy(Dictionary<STATS, int> wages, TimeSpend timeSpend)
        {
            switch (timeSpend.ProjectType)
            {
                case (PROJECT_TYPE.ENERGY):
                    return calculateEnergyProjectEnergy(wages, timeSpend.Duration);
                case (PROJECT_TYPE.STATS):
                    return calculateStatsProjectEnergy(wages, timeSpend.Duration);

                default:
                    return null;
            }

        }

        private DailyEnergy calculateEnergyProjectEnergy(Dictionary<STATS, int> wages, int duration)
        {
            duration >>= 2;
            int sum = wages.Sum(x => x.Value);
            return new DailyEnergy
            {
                Body = duration * wages.GetValueOrDefault(STATS.STRENGTH) / sum,
                Soul = duration * wages.GetValueOrDefault(STATS.FLUENCY) / sum,
                Emotions = duration * wages.GetValueOrDefault(STATS.CREATIVITY) / sum,
                Mind = duration * wages.GetValueOrDefault(STATS.INTELLIGENCE) / sum
            };
        }

        private DailyEnergy calculateStatsProjectEnergy(Dictionary<STATS, int> wages, int duration)
        {
            duration *= -1;
            int sum = wages.Sum(x => x.Value);
            return new DailyEnergy
            {
                Body = duration * wages.GetValueOrDefault(STATS.STRENGTH) / sum,
                Soul = duration * wages.GetValueOrDefault(STATS.FLUENCY) / sum,
                Emotions = duration * wages.GetValueOrDefault(STATS.CREATIVITY) / sum,
                Mind = duration * wages.GetValueOrDefault(STATS.INTELLIGENCE) / sum
            };

        }

        private DailyStats calculateStats(Dictionary<STATS, int> wages, TimeSpend timeSpend)
        {
            switch (timeSpend.ProjectType)
            {
                case (PROJECT_TYPE.ENERGY):
                    return calculateEnergyProjectStats(wages, timeSpend.Duration);
                case (PROJECT_TYPE.STATS):
                    return calculateStatsProjectStats(wages, timeSpend.Duration);

                default:
                    return null;
            }
        }

        private DailyStats calculateEnergyProjectStats(Dictionary<STATS, int> wages, int duration)
        {
            duration >>= 2;
            int sum = wages.Sum(x => x.Value);
            return new DailyStats
            {
                Strength = duration * wages.GetValueOrDefault(STATS.STRENGTH) / sum,
                Creativity = duration * wages.GetValueOrDefault(STATS.CREATIVITY) / sum,
                Fluency = duration * wages.GetValueOrDefault(STATS.FLUENCY) / sum,
                Intelligence = duration * wages.GetValueOrDefault(STATS.INTELLIGENCE) / sum
            };
        }

        private DailyStats calculateStatsProjectStats(Dictionary<STATS, int> wages, int duration)
        {

            int sum = wages.Sum(x => x.Value);
            return new DailyStats
            {
                Strength = duration * wages.GetValueOrDefault(STATS.STRENGTH) / sum,
                Creativity = duration * wages.GetValueOrDefault(STATS.CREATIVITY) / sum,
                Fluency = duration * wages.GetValueOrDefault(STATS.FLUENCY) / sum,
                Intelligence = duration * wages.GetValueOrDefault(STATS.INTELLIGENCE) / sum
            };
        }

    }
}