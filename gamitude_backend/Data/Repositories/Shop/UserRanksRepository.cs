using gamitude_backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using gamitude_backend.Data;
using MongoDB.Bson;
using System.Linq;

namespace gamitude_backend.Repositories
{
    public interface IUserRanksRepository
    {
        Task<List<string>> getByUserIdAsync(string userId);
        Task addAsync(string userId, string rankId);
        Task addListAsync(string userId, List<string> rankIds);
    }
    public class UserRanksRepository : IUserRanksRepository
    {
        private readonly IMongoCollection<User> _users;


        public UserRanksRepository(IDatabaseCollections dbCollections)
        {
            _users = dbCollections.users;
        }

        public Task addAsync(string userId, string rankId)
        {
            var filter = Builders<User>.Filter.Eq("_id", new ObjectId(userId));
            var update = Builders<User>.Update.AddToSet("purchasedRankIds", rankId);
            return _users.UpdateOneAsync(filter, update);
        }

        public Task addListAsync(string userId, List<string> rankIds)
        {
            var filter = Builders<User>.Filter.Eq("_id", new ObjectId(userId));
            var update = Builders<User>.Update.AddToSet("purchasedRankIds", rankIds.Select(o => new ObjectId(o)).ToList());
            return _users.UpdateOneAsync(filter, update);
        }

        public async Task<List<string>> getByUserIdAsync(string userId)
        {
            var projection = Builders<User>.Projection.Include("purchasedRankIds").Exclude("_id");
            var filter = Builders<User>.Filter.Eq("_id", new ObjectId(userId));
            var result = await _users.Find(filter).Project(projection).FirstOrDefaultAsync();
            result.TryGetValue("purchasedRankIds", out var ranks);
            return ranks.AsBsonArray.Select(o => o.AsObjectId.ToString()).ToList();

        }
    }
}