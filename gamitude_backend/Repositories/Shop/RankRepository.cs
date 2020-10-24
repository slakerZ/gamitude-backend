using gamitude_backend.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using gamitude_backend.Settings;
using MongoDB.Driver;
using gamitude_backend.Data;

namespace gamitude_backend.Repositories
{
    public interface IRankRepository
    {
        Task<Rank> getByIdAsync(String id);
        Task<Rank> getRookieAsync();
        Task createAsync(Rank rank);
        Task updateAsync(String id, Rank updateRank);
        Task deleteByIdAsync(String id);
    }
    public class RankRepository : IRankRepository
    {
        private readonly IMongoCollection<Rank> _ranks;


        public RankRepository(IDatabaseCollections dbCollections)
        {
            _ranks = dbCollections.ranks;
        }

        public Task<Rank> getByIdAsync(String id)
        {
            return _ranks.Find<Rank>(Rank => Rank.id == id).FirstOrDefaultAsync();
        }

        public Task createAsync(Rank Rank)
        {
            return _ranks.InsertOneAsync(Rank);
        }

        public Task updateAsync(String id, Rank newRank)
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
    }
}