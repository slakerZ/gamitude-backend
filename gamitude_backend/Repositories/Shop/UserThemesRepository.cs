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
    public interface IUserThemesRepository
    {
        Task<UserThemes> getByIdAsync(String id);
        Task<List<UserThemes>> getByUserIdAsync(String userId);
        Task createAsync(UserThemes rank);
        Task updateAsync(String id, UserThemes updateUserThemes);
        Task deleteByIdAsync(String id);
    }
    public class UserThemesRepository : IUserThemesRepository
    {
        private readonly IMongoCollection<UserThemes> _userThemes;


        public UserThemesRepository(IDatabaseCollections dbCollections)
        {
            _userThemes = dbCollections.userThemes;
        }

        public Task<UserThemes> getByIdAsync(String id)
        {
            return _userThemes.Find<UserThemes>(UserThemes => UserThemes.id == id).FirstOrDefaultAsync();
        }

        public Task createAsync(UserThemes UserThemes)
        {
            return _userThemes.InsertOneAsync(UserThemes);
        }

        public Task updateAsync(String id, UserThemes newUserThemes)
        {
            return _userThemes.ReplaceOneAsync(UserThemes => UserThemes.id == id, newUserThemes);

        }

        public Task deleteByUserThemesAsync(UserThemes UserThemesIn)
        {
            return _userThemes.DeleteOneAsync(UserThemes => UserThemes.id == UserThemesIn.id);
        }

        public Task deleteByIdAsync(string id)
        {
            return _userThemes.DeleteOneAsync(UserThemes => UserThemes.id == id);

        }

        public Task<List<UserThemes>> getByUserIdAsync(string userId)
        {
            return _userThemes.Find<UserThemes>(userThemes => userThemes.userId == userId).ToListAsync();
        }
    }
}