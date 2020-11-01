using gamitude_backend.Models;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using gamitude_backend.Dto.Energy;
using gamitude_backend.Settings;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using gamitude_backend.Repositories;
using gamitude_backend.Data;

namespace gamitude_backend.Services
{

    public interface IDailyEnergyService : IDailyEnergyRepository
    {
        Task<GetLastWeekAvgEnergyDto> GetLastWeekAvgEnergyByUserIdAsync(String userId);

    }

    public class DailyEnergyService : DailyEnergyRepository, IDailyEnergyService
    {
        private readonly IMongoCollection<DailyEnergy> _DailyEnergy;
        private readonly ILogger<DailyEnergyService> _logger;

        public DailyEnergyService(IDatabaseCollections dbCollections, ILogger<DailyEnergyService> logger) : base(dbCollections)
        {

            _DailyEnergy = dbCollections.dailyEnergies;
            _logger = logger;
        }
        /// <summary>
        /// Gets last weeks energy counting the days for further calculations
        /// </summary>
        public async Task<GetLastWeekAvgEnergyDto> GetLastWeekAvgEnergyByUserIdAsync(String userId)
        {
            GetLastWeekAvgEnergyDto energy = await _DailyEnergy.AsQueryable()
                 .Where(o => o.userId == userId && o.dateCreated > DateTime.UtcNow.Date.AddDays(-7))
                 .GroupBy(o => o.userId)
                 .Select(o => new GetLastWeekAvgEnergyDto
                 {
                     body = o.Sum(o => o.body),
                     mind = o.Sum(o => o.mind),
                     emotions = o.Sum(o => o.emotions),
                     soul = o.Sum(o => o.soul),
                     dayCount = o.Sum(o => 1)

                 }).FirstOrDefaultAsync() ?? new GetLastWeekAvgEnergyDto();

            return energy.weekAvg().scaleToPercent();
        }
    }
}