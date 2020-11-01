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
    public interface IUserRankRepository
    {
        Task<UserRank> getByIdAsync(String id);
        Task<UserRank> getByUserIdAsync(String userId);
        Task createOrUpdateAsync(UserRank rank);
    }
    public class UserRankRepository : IUserRankRepository
    {
        private readonly IMongoCollection<UserRank> _userRank;


        public UserRankRepository(IDatabaseCollections dbCollections)
        {
            _userRank = dbCollections.userRank;
        }

        public Task<UserRank> getByIdAsync(String id)
        {
            return _userRank.Find<UserRank>(UserRank => UserRank.id == id).FirstOrDefaultAsync();
        }

        public Task<UserRank> getByUserIdAsync(String userId)
        {
            return _userRank.Find<UserRank>(userRank => userRank.userId == userId).FirstOrDefaultAsync();
        }

         public Task createOrUpdateAsync(UserRank UserRank)
        {
            return _userRank.ReplaceOneAsync(o => o.id == UserRank.id, UserRank, new ReplaceOptions{IsUpsert=true});
        }
    }
}