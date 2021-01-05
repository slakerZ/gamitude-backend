using gamitude_backend.Models;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using gamitude_backend.Data;

namespace gamitude_backend.Repositories
{
    public interface IDailyEnergyRepository
    {
        Task<DailyEnergy> getByIdAsync(string id);
        Task<DailyEnergy> getByDateAndUserIdAsync(DateTime date, string userId);
        Task<List<DailyEnergy>> getByUserIdAsync(string userId);
        Task createAsync(DailyEnergy dailyEnergy);
        Task createOrUpdateAsync(DailyEnergy dailyEnergy);
        Task updateAsync(string id, DailyEnergy updateDailyEnergy);
        Task deleteByIdAsync(string id);
    }
    public class DailyEnergyRepository : IDailyEnergyRepository
    {
        private readonly IMongoCollection<DailyEnergy> _DailyEnergies;


        public DailyEnergyRepository(IDatabaseCollections dbCollections)
        {
            _DailyEnergies = dbCollections.dailyEnergies;
        }

        public Task<DailyEnergy> getByIdAsync(string id)
        {
            return _DailyEnergies.Find<DailyEnergy>(DailyEnergy => DailyEnergy.id == id).FirstOrDefaultAsync();
        }
        public Task<DailyEnergy> getByDateAndUserIdAsync(DateTime date, string userId)
        {
            return _DailyEnergies.Find(o => o.dateCreated == date.Date && o.userId == userId).SingleOrDefaultAsync();

        }

        public Task<List<DailyEnergy>> getByUserIdAsync(string userId)
        {
            return _DailyEnergies.Find<DailyEnergy>(DailyEnergy => DailyEnergy.userId == userId).ToListAsync();

        }

        public Task createAsync(DailyEnergy DailyEnergy)
        {
            return _DailyEnergies.InsertOneAsync(DailyEnergy.validate());
        }

        public Task updateAsync(string id, DailyEnergy newDailyEnergy)
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
        public Task createOrUpdateAsync(DailyEnergy dailyEnergy)
        {
            if (dailyEnergy.id != null)
            {
                return updateAsync(dailyEnergy.id, dailyEnergy.validate());
            }
            return createAsync(dailyEnergy.validate());

        }

        /// <summary>
        /// Helper function for summing DailyEnergy could be later replaced with automapper
        /// </summary>
        private DailyEnergy mergeDailyEnergy(DailyEnergy dailyEnergy, DailyEnergy oldDEnergy)
        {
            dailyEnergy.body += oldDEnergy.body;
            dailyEnergy.emotions += oldDEnergy.emotions;
            dailyEnergy.mind += oldDEnergy.mind;
            dailyEnergy.soul += oldDEnergy.soul;
            return dailyEnergy;
        }
    }
}