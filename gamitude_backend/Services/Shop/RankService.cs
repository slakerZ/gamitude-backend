using gamitude_backend.Models;

using MongoDB.Driver;
using gamitude_backend.Repositories;
using gamitude_backend.Data;
using System.Threading.Tasks;

namespace gamitude_backend.Services
{


    public interface IRankService : IRankRepository
    {
    }

    public class RankService : RankRepository, IRankService
    {
        private readonly IMongoCollection<Rank> _ranks;

        public RankService(IDatabaseCollections dbCollections) : base(dbCollections)
        {
            _ranks = dbCollections.ranks;
        }

    }
}