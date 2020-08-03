using gamitude_backend.Models;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using gamitude_backend.Dto.Energy;
using gamitude_backend.Settings;
using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace gamitude_backend.Services
{

    public interface IDailyEnergyService
    {
        Task<GetLastWeekAvgEnergyDto> GetLastWeekAvgEnergyByUserIdAsync(String userId);
        Task<GetDailyEnergyDto> GetDailyEnergyByUserIdAsync(String userId);
        DailyEnergy GetDailyEnergyByUserId(String userId);
        DailyEnergy Get(String id);
        DailyEnergy Create(DailyEnergy dailyEnergy);
        Task<DailyEnergy> CreateOrAddAsync(DailyEnergy dailyEnergy);
        Task UpdateAsync(DailyEnergy dailyEnergy);
        void Update(DailyEnergy dailyEnergy);
        void Remove(DailyEnergy dailyEnergy);
        void Remove(String id);

    }

    public class DailyEnergyService : IDailyEnergyService
    {
        private readonly IMongoCollection<DailyEnergy> _DailyEnergy;
        private readonly IMapper _mapper;
        private readonly ILogger<DailyEnergyService> _logger;

        public DailyEnergyService(IMapper mapper, IDatabaseSettings settings, ILogger<DailyEnergyService> logger)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _DailyEnergy = database.GetCollection<DailyEnergy>(settings.DailyEnergyCollectionName);
            _mapper = mapper;
            _logger = logger;
        }
        /// <summary>
        /// Gets last weeks energy counting the days for further calculations
        /// </summary>
        public async Task<GetLastWeekAvgEnergyDto> GetLastWeekAvgEnergyByUserIdAsync(String userId)
        {
            GetLastWeekAvgEnergyDto energy = await _DailyEnergy.AsQueryable()
                 .Where(o => o.UserId == userId && o.Date > DateTime.UtcNow.Date.AddDays(-7))
                 .GroupBy(o => o.UserId)
                 .Select(o => new GetLastWeekAvgEnergyDto
                 {
                     Body = o.Sum(o => o.Body),
                     Mind = o.Sum(o => o.Mind),
                     Emotions = o.Sum(o => o.Emotions),
                     Soul = o.Sum(o => o.Soul),
                     dayCount = o.Sum(o => 1)

                 }).FirstOrDefaultAsync() ?? new GetLastWeekAvgEnergyDto();
                 
            return energy.weekAvg().scaleToPercent();
        }
        public async Task<GetDailyEnergyDto> GetDailyEnergyByUserIdAsync(string userId)
        {
            var energy = await _DailyEnergy.Find<DailyEnergy>(o => o.Date == DateTime.UtcNow.Date && o.UserId == userId).FirstOrDefaultAsync();
            return energy != null ? _mapper.Map<GetDailyEnergyDto>(energy) : new GetDailyEnergyDto(); 
        }


        public DailyEnergy GetDailyEnergyByUserId(String userId) =>
            _DailyEnergy.Find<DailyEnergy>(o => o.UserId == userId).FirstOrDefault();

        public DailyEnergy Get(String id) =>
            _DailyEnergy.Find<DailyEnergy>(o => o.Id == id).FirstOrDefault();


        public async Task<DailyEnergy> CreateOrAddAsync(DailyEnergy dailyEnergy)
        {
            try
            {
                var oldDEnergy = await _DailyEnergy.Find(o => o.Date == dailyEnergy.Date && o.UserId == dailyEnergy.UserId).SingleOrDefaultAsync();
                if (null == oldDEnergy)
                {//Create
                    await _DailyEnergy.InsertOneAsync(mergeDailyEnergy(dailyEnergy, new DailyEnergy().init()).validate());
                }
                else
                {//Update   //TODO use mapper for adding
                    dailyEnergy.Id = oldDEnergy.Id;
                    dailyEnergy = mergeDailyEnergy(dailyEnergy, oldDEnergy);
                    await UpdateAsync(dailyEnergy.validate());
                }
                return dailyEnergy;
            }
            catch (Exception e)
            {
                _logger.LogError("Error cached in DailyEnergyServices CreateOrUpdateAsync {error}", e);
                return null;
            }

        }

        public DailyEnergy Create(DailyEnergy DailyEnergy)
        {
            _DailyEnergy.InsertOne(DailyEnergy);
            return DailyEnergy;
        }

        public void Update(DailyEnergy dailyEnergy) =>
            _DailyEnergy.ReplaceOne(o => o.Id == dailyEnergy.Id, dailyEnergy);

        public async Task UpdateAsync(DailyEnergy dailyEnergy) =>
            await _DailyEnergy.ReplaceOneAsync(o => o.Id == dailyEnergy.Id, dailyEnergy);

        public void Remove(DailyEnergy dailyEnergy) =>
            _DailyEnergy.DeleteOne(o => o.Id == dailyEnergy.Id);

        public void Remove(String id) =>
            _DailyEnergy.DeleteOne(o => o.Id == id);

        /// <summary>
        /// Helper function for summing DailyEnergy could be later replaced with automapper
        /// </summary>
        private DailyEnergy mergeDailyEnergy(DailyEnergy dailyEnergy, DailyEnergy oldDEnergy) 
        {
            dailyEnergy.Body += oldDEnergy.Body;
            dailyEnergy.Emotions += oldDEnergy.Emotions;
            dailyEnergy.Mind += oldDEnergy.Mind;
            dailyEnergy.Soul += oldDEnergy.Soul;
            return dailyEnergy;
        }


    }
}