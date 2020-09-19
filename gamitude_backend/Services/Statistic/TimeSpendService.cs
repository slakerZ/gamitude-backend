using gamitude_backend.Models;
 
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using gamitude_backend.Settings;
using MongoDB.Driver;

namespace gamitude_backend.Services
{

    public interface ITimeSpendService
    {
        TimeSpend GetTimeSpendByProjectId(String userId);
        TimeSpend Get(String id);
        TimeSpend Create(TimeSpend timeSpend);
        Task<TimeSpend> CreateAsync(TimeSpend timeSpend);
        void Update(TimeSpend timeSpend);
        void Remove(TimeSpend timeSpend);
        void Remove(string id);


    }

    public class TimeSpendService : ITimeSpendService
    {
        private readonly IMongoCollection<TimeSpend> _TimeSpend;


        public TimeSpendService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.connectionString);
            var database = client.GetDatabase(settings.databaseName);

            _TimeSpend = database.GetCollection<TimeSpend>(settings.timeSpendCollectionName);
        }

        public TimeSpend GetTimeSpendByProjectId(string projectId) =>
            _TimeSpend.Find<TimeSpend>(TimeSpend => TimeSpend.ProjectId == projectId).FirstOrDefault();

        public TimeSpend Get(string id) =>
            _TimeSpend.Find<TimeSpend>(TimeSpend => TimeSpend.Id == id).FirstOrDefault();

        public TimeSpend Create(TimeSpend TimeSpend)
        {
            _TimeSpend.InsertOne(TimeSpend);
            return TimeSpend;
        }

        public async Task<TimeSpend> CreateAsync(TimeSpend TimeSpend)
        {
            await _TimeSpend.InsertOneAsync(TimeSpend);
            return TimeSpend;
        }

        public void Update(TimeSpend timeSpend) =>
            _TimeSpend.ReplaceOne(TimeSpend => TimeSpend.Id == timeSpend.Id, timeSpend);

        public void Remove(TimeSpend timeSpend) =>
            _TimeSpend.DeleteOne(TimeSpend => TimeSpend.Id == timeSpend.Id);

        public void Remove(string id) => 
            _TimeSpend.DeleteOne(TimeSpend => TimeSpend.Id == id);

    }
}