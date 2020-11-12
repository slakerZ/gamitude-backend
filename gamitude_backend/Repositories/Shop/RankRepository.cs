using gamitude_backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using gamitude_backend.Data;
using gamitude_backend.Extensions;

namespace gamitude_backend.Repositories
{
    public interface IRankRepository
    {
        Task<Rank> getByIdAsync(string id);
        Task<List<Rank>> getByIdAsync(List<string> ids);
        Task<IReadOnlyList<Rank>> getAllAsync(int page = 1, int limit = 20, string sortBy = "name");
        Task<Rank> getRookieAsync();
        Task<List<Rank>> getAsync();
        Task createAsync(Rank rank);
        Task updateAsync(string id, Rank updateRank);
        Task deleteByIdAsync(string id);
    }
    public class RankRepository : IRankRepository
    {
        private readonly IMongoCollection<Rank> _ranks;


        public RankRepository(IDatabaseCollections dbCollections)
        {
            _ranks = dbCollections.ranks;
        }

        public Task<Rank> getByIdAsync(string id)
        {
            return _ranks.Find<Rank>(Rank => Rank.id == id).FirstOrDefaultAsync();
        }
        public Task<List<Rank>> getAsync()
        {
            return _ranks.Find<Rank>(FilterDefinition<Rank>.Empty).ToListAsync();
        }
        public Task<List<Rank>> getByIdAsync(List<string> ids)
        {
            return _ranks.Find<Rank>(Rank => ids.Contains(Rank.id)).ToListAsync();
        }

        public Task createAsync(Rank Rank)
        {
            return _ranks.InsertOneAsync(Rank);
        }

        public Task updateAsync(string id, Rank newRank)
        {
            return _ranks.ReplaceOneAsync(Rank => Rank.id == id, newRank);

        }

        public Task deleteByRankAsync(Rank RankIn)
        {
            return _ranks.DeleteOneAsync(Rank => Rank.id == RankIn.id);
        }

        public Task deleteByIdAsync(string id)
        {
            return _ranks.DeleteOneAsync(Rank => Rank.id == id);

        }

        public Task<Rank> getRookieAsync()
        {
            return _ranks.Find<Rank>(o => o.rookie == true).FirstOrDefaultAsync();
        }

        public Task<IReadOnlyList<Rank>> getAllAsync(int page = 1, int limit = 20, string sortBy = "name")
        {
            return _ranks.AggregateByPage<Rank>(Builders<Rank>.Filter.Empty,
                                            Builders<Rank>.Sort.Ascending(x => x.GetType()
                                                                                .GetProperty(sortBy)
                                                                                .GetValue(x, null)), page, limit);
        }

    }
}