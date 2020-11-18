using gamitude_backend.Models;
using System.Threading.Tasks;
using MongoDB.Driver;
using gamitude_backend.Data;
using MongoDB.Bson;

namespace gamitude_backend.Repositories
{
    public interface IUserThemeRepository
    {
        Task<string> getByUserIdAsync(string userId);
        Task createOrUpdateAsync(string userId, string themeId);
    }
    public class UserThemeRepository : IUserThemeRepository
    {
        private readonly IMongoCollection<User> _users;


        public UserThemeRepository(IDatabaseCollections dbCollections)
        {
            _users = dbCollections.users;
        }

        public async Task<string> getByUserIdAsync(string userId)
        {

            var projection = Builders<User>.Projection.Include("currentThemeId").Exclude("_id");
            var filter = Builders<User>.Filter.Eq("_id", new ObjectId(userId));
            var result = await _users.Find(filter).Project(projection).FirstOrDefaultAsync();
            result.TryGetValue("currentThemeId", out var theme);
            return theme.ToString();

        }

        public Task createOrUpdateAsync(string userId, string themeId)
        {
            var filter = Builders<User>.Filter.Eq("_id", new ObjectId(userId));
            var update = Builders<User>.Update.Set("currentThemeId", new ObjectId(themeId));
            return _users.UpdateOneAsync(filter, update);
        }
    }
}