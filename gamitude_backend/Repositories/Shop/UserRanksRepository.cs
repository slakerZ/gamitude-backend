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
    public interface IUserRanksRepository
    {
        Task<UserRanks> getByIdAsync(String id);
        Task<UserRanks> getByUserIdAsync(String userId);
        Task createAsync(UserRanks rank);
        Task updateAsync(String id, UserRanks updateUserRanks);
        Task deleteByIdAsync(String id);
    }
    public class UserRanksRepository : IUserRanksRepository
    {
        private readonly IMongoCollection<UserRanks> _userRanks;


        public UserRanksRepository(IDatabaseCollections dbCollections)
        {
            _userRanks = dbCollections.userRanks;
        }

        public Task<UserRanks> getByIdAsync(String id)
        {
            return _userRanks.Find<UserRanks>(UserRanks => UserRanks.id == id).FirstOrDefaultAsync();
        }

        public Task createAsync(UserRanks UserRanks)
        {
            return _userRanks.InsertOneAsync(UserRanks);
        }

        public Task updateAsync(String id, UserRanks newUserRanks)
        {
            return _userRanks.ReplaceOneAsync(UserRanks => UserRanks.id == id, newUserRanks);

        }

        public Task deleteByUserRanksAsync(UserRanks UserRanksIn)
        {
            return _userRanks.DeleteOneAsync(UserRanks => UserRanks.id == UserRanksIn.id);
        }

        public Task deleteByIdAsync(string id)
        {
            return _userRanks.DeleteOneAsync(UserRanks => UserRanks.id == id);

        }

        public Task<UserRanks> getByUserIdAsync(string userId)
        {
            return _userRanks.Find<UserRanks>(userRanks => userRanks.userId == userId).FirstOrDefaultAsync();
        }
    }
}