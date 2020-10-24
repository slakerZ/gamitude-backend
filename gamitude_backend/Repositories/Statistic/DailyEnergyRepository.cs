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
    public interface IDailyEnergyRepository
    {
        Task<DailyEnergy> getByIdAsync(String id);
        Task<List<DailyEnergy>> getByUserIdAsync(String userId);
        Task createAsync(DailyEnergy dailyEnergy);
        Task updateAsync(String id, DailyEnergy updateDailyEnergy);
        Task deleteByIdAsync(String id);
    }
    public class DailyEnergyRepository : IDailyEnergyRepository
    {
        private readonly IMongoCollection<DailyEnergy> _DailyEnergies;


        public DailyEnergyRepository(IDatabaseCollections dbCollections)
        {
            _DailyEnergies = dbCollections.dailyEnergies;
        }

        public Task<DailyEnergy> getByIdAsync(String id)
        {
            return _DailyEnergies.Find<DailyEnergy>(DailyEnergy => DailyEnergy.id == id).FirstOrDefaultAsync();
        }

        public Task<List<DailyEnergy>> getByUserIdAsync(String userId)
        {
            return _DailyEnergies.Find<DailyEnergy>(DailyEnergy => DailyEnergy.userId == userId).ToListAsync();

        }

        public Task createAsync(DailyEnergy DailyEnergy)
        {
            return _DailyEnergies.InsertOneAsync(DailyEnergy);
        }

        public Task updateAsync(String id, DailyEnergy newDailyEnergy)
        {
            return _DailyEnergies.ReplaceOneAsync(DailyEnergy => DailyEnergy.id == id, newDailyEnergy);

        }

        public Task deleteByDailyEnergyAsync(DailyEnergy DailyEnergyIn)
        {
            return _DailyEnergies.DeleteOneAsync(DailyEnergy => DailyEnergy.id == DailyEnergyIn.id);
        }

        public Task deleteByIdAsync(string id)
        {
            return _DailyEnergies.DeleteOneAsync(DailyEnergy => DailyEnergy.id == id);

        }

    }
}