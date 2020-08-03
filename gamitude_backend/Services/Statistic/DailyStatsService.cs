using gamitude_backend.Models;
 
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using gamitude_backend.Dto.Stats;
 
using gamitude_backend.Settings;
using AutoMapper;

namespace gamitude_backend.Services
{

    public interface IDailyStatsService
    {
        Task<GetLastWeekAvgStatsDto> GetLastWeekAvgStatsByUserIdAsync(String userId);
        Task<GetDailyStatsDto> GetDailyStatsByUserIdAsync(String userId);
        DailyStats GetDailyStatsByUserId(String userId);
        DailyStats Get(String id);
        DailyStats Create(DailyStats dailyStats);
        Task<DailyStats> CreateOrAddAsync(DailyStats dailyStats);
        void Update(DailyStats dailyStats);
        Task UpdateAsync(DailyStats dailyStats);
        void Remove(DailyStats dailyStats);
        void Remove(string id);

    }

    public class DailyStatsService : IDailyStatsService
    {
        private readonly IMongoCollection<DailyStats> _DailyStats;
        private readonly IMapper _mapper;
        private readonly ILogger<DailyStatsService> _logger;

        public DailyStatsService(IMapper mapper, IDatabaseSettings settings, ILogger<DailyStatsService> logger)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _DailyStats = database.GetCollection<DailyStats>(settings.DailyStatsCollectionName);
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetLastWeekAvgStatsDto> GetLastWeekAvgStatsByUserIdAsync(String userId)
        {
            GetLastWeekAvgStatsDto stats = await _DailyStats.AsQueryable()
                 .Where(o => o.UserId == userId && o.Date > DateTime.UtcNow.Date.AddDays(-7))
                 .GroupBy(o => o.UserId)
                 .Select(o => new GetLastWeekAvgStatsDto
                 {
                     Creativity = o.Sum(o => o.Creativity),
                     Fluency = o.Sum(o => o.Fluency),
                     Intelligence = o.Sum(o => o.Intelligence),
                     Strength = o.Sum(o => o.Strength)

                 }).FirstOrDefaultAsync() ?? new GetLastWeekAvgStatsDto();
            return stats.weekAvg().scaleToPercent();
        }
        public async Task<GetDailyStatsDto> GetDailyStatsByUserIdAsync(string userId)
        {
            var stats = await _DailyStats.Find<DailyStats>(o => o.Date == DateTime.UtcNow.Date && o.UserId == userId).FirstOrDefaultAsync();
            return stats != null ? _mapper.Map<GetDailyStatsDto>(stats) : new GetDailyStatsDto();
        }

        public DailyStats GetDailyStatsByUserId(string userId) =>
            _DailyStats.Find<DailyStats>(DailyStats => DailyStats.UserId == userId).FirstOrDefault();

        public DailyStats Get(string id) =>
            _DailyStats.Find<DailyStats>(DailyStats => DailyStats.Id == id).FirstOrDefault();

        public DailyStats Create(DailyStats DailyStats)
        {
            _DailyStats.InsertOne(DailyStats);
            return DailyStats;
        }

        public async Task<DailyStats> CreateOrAddAsync(DailyStats dailyStats)
        {
            try
            {
                var oldDEnergy = await _DailyStats.Find(o => o.Date == dailyStats.Date && o.UserId == dailyStats.UserId).SingleOrDefaultAsync();
                if (null == oldDEnergy)
                {//Create
                    await _DailyStats.InsertOneAsync(dailyStats);
                }
                else
                {//Update
                    dailyStats.Id = oldDEnergy.Id;
                    dailyStats.Creativity += oldDEnergy.Creativity;
                    dailyStats.Fluency += oldDEnergy.Fluency;
                    dailyStats.Intelligence += oldDEnergy.Intelligence;
                    dailyStats.Strength += oldDEnergy.Strength;
                    await UpdateAsync(dailyStats);
                }
                return dailyStats;
            }
            catch (Exception e)
            {
                _logger.LogError("Error cached in DailyEnergyServices CreateOrUpdateAsync {error}", e);
                return null;
            }

        }

        public void Update(DailyStats dailyStats) =>
            _DailyStats.ReplaceOne(o => o.Id == dailyStats.Id, dailyStats);

        public async Task UpdateAsync(DailyStats dailyStats) =>
            await _DailyStats.ReplaceOneAsync(o => o.Id == dailyStats.Id, dailyStats);
        public void Remove(DailyStats dailyStats) =>
            _DailyStats.DeleteOne(o => o.Id == dailyStats.Id);

        public void Remove(string id) =>
            _DailyStats.DeleteOne(o => o.Id == id);


    }
}