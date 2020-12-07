using gamitude_backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using gamitude_backend.Data;
using gamitude_backend.Extensions;
using MongoDB.Bson;

namespace gamitude_backend.Repositories
{
    public interface IMoneyRepository
    {
        Task<long> getMoneyByUserIdAsync(string userId);
        Task createOrUpdateMoneyAsync(string userId, long newMoney);
        
    }
    public class MoneyRepository : IMoneyRepository
    {
        private readonly IMongoCollection<User> _users;

        public MoneyRepository(IDatabaseCollections dbCollections)
        {
            _users = dbCollections.users;
        }

        public async Task<long> getMoneyByUserIdAsync(string userId)
        {
            var projection = Builders<User>.Projection.Include("money").Exclude("_id");
            var filter = Builders<User>.Filter.Eq("_id",  new ObjectId(userId));
            var result = await _users.Find(filter).Project(projection).FirstOrDefaultAsync();
            result.TryGetValue("money", out var money);
            return money.ToInt64();
        }

        public Task createOrUpdateMoneyAsync(string userId, long newMoney)
        {
            var filter = Builders<User>.Filter.Eq("_id", new ObjectId(userId));
            var update = Builders<User>.Update.Set("money", newMoney);
            return _users.UpdateOneAsync(filter, update);
        }
    }
}