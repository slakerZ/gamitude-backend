using gamitude_backend.Models;
using System.Threading.Tasks;
using MongoDB.Driver;
using gamitude_backend.Data;
using MongoDB.Bson;

namespace gamitude_backend.Repositories
{
    public interface IUserRankRepository
    {
        Task<string> getByUserIdAsync(string userId);
        Task createOrUpdateAsync(string userId, string rankId);
    }
    public class UserRankRepository : IUserRankRepository
    {
        private readonly IMongoCollection<User> _users;


        public UserRankRepository(IDatabaseCollections dbCollections)
        {
            _users = dbCollections.users;
        }

        public async Task<string> getByUserIdAsync(string userId)
        {
            var projection = Builders<User>.Projection.Include("currentRankId").Exclude("_id");
            var filter = Builders<User>.Filter.Eq("_id",  new ObjectId(userId));
            var result = await _users.Find(filter).Project(projection).FirstOrDefaultAsync();
            result.TryGetValue("currentRankId", out var rank);
            return rank.ToString();
        }

        public Task createOrUpdateAsync(string userId, string rankId)
        {
            var filter = Builders<User>.Filter.Eq("_id", new ObjectId(userId));
            var update = Builders<User>.Update.Set("currentRankId", new ObjectId(rankId));
            return _users.UpdateOneAsync(filter, update);
        }
    }
}