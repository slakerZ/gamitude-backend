using gamitude_backend.Models;

using MongoDB.Driver;
using gamitude_backend.Repositories;
using gamitude_backend.Data;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace gamitude_backend.Services
{


    public interface IRankService : IRankRepository
    {
        Task<Rank> getCurrentByUserIdAsync(String id);
        Task<IReadOnlyList<Rank>> getAllByUserIdAsync(String id);
    }

    public class RankService : RankRepository, IRankService
    {
        private readonly IMongoCollection<Rank> _ranks;
        private readonly IUserRankRepository _userRankRepository;
        private readonly IUserRanksRepository _userRanksRepository;

        public RankService(IDatabaseCollections dbCollections,IUserRankRepository userRankRepository, IUserRanksRepository userRanksRepository) : base(dbCollections)
        {
            _ranks = dbCollections.ranks;
            _userRankRepository = userRankRepository;
            _userRanksRepository = userRanksRepository;
        }
        public async Task<Rank> getCurrentByUserIdAsync(string id)
        {
            var userRank = await  _userRankRepository.getByUserIdAsync(id);
            return await getByIdAsync(userRank.rankId);
        }
        public async Task<IReadOnlyList<Rank>> getAllByUserIdAsync(string id)
        {
            var userRanks = await  _userRanksRepository.getByUserIdAsync(id);
            return await getByIdAsync(userRanks.rankIds);
        }
    }
}