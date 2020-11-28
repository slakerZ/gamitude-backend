using gamitude_backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using gamitude_backend.Data;
using MongoDB.Bson;
using System.Linq;

namespace gamitude_backend.Repositories
{
    public interface IUserThemesRepository
    {
        Task<List<string>> getByUserIdAsync(string userId);
        Task addAsync(string userId, string themeId);
        Task addListAsync(string userId, List<string> themeIds);
    }
    public class UserThemesRepository : IUserThemesRepository
    {
        private readonly IMongoCollection<User> _users;



        public UserThemesRepository(IDatabaseCollections dbCollections)
        {
            _users = dbCollections.users;
        }
        public Task addAsync(string userId, string themeId)
        {
            var filter = Builders<User>.Filter.Eq("_id", userId);
            var update = Builders<User>.Update.AddToSet("purchasedThemeIds", new ObjectId(themeId));
            return _users.UpdateOneAsync(filter, update);
        }

        public Task addListAsync(string userId, List<string> themeIds)
        {
            var filter = Builders<User>.Filter.Eq("_id", new ObjectId(userId));
            var update = Builders<User>.Update.AddToSet("purchasedThemeIds", themeIds.Select(o => new ObjectId(o)).ToList());
            return _users.UpdateOneAsync(filter, update);
        }

        public async Task<List<string>> getByUserIdAsync(string userId)
        {
            var projection = Builders<User>.Projection.Include("purchasedThemeIds").Exclude("_id");
            var filter = Builders<User>.Filter.Eq("_id", new ObjectId(userId));
            var result = await _users.Find(filter).Project(projection).FirstOrDefaultAsync();
            result.TryGetValue("purchasedThemeIds", out var ranks);
            return ranks.AsBsonArray.Select(o => o.AsObjectId.ToString()).ToList();
        }
    }
}