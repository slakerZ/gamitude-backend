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
    public interface IUserThemeRepository
    {
        Task<UserTheme> getByIdAsync(String id);
        Task<UserTheme> getByUserIdAsync(String userId);
        Task createAsync(UserTheme rank);
        Task updateAsync(String id, UserTheme updateUserTheme);
        Task deleteByIdAsync(String id);
    }
    public class UserThemeRepository : IUserThemeRepository
    {
        private readonly IMongoCollection<UserTheme> _userTheme;


        public UserThemeRepository(IDatabaseCollections dbCollections)
        {
            _userTheme = dbCollections.userTheme;
        }

        public Task<UserTheme> getByIdAsync(String id)
        {
            return _userTheme.Find<UserTheme>(UserTheme => UserTheme.id == id).FirstOrDefaultAsync();
        }

        public Task createAsync(UserTheme UserTheme)
        {
            return _userTheme.InsertOneAsync(UserTheme);
        }

        public Task updateAsync(String id, UserTheme newUserTheme)
        {
            return _userTheme.ReplaceOneAsync(UserTheme => UserTheme.id == id, newUserTheme);

        }

        public Task deleteByUserThemeAsync(UserTheme UserThemeIn)
        {
            return _userTheme.DeleteOneAsync(UserTheme => UserTheme.id == UserThemeIn.id);
        }

        public Task deleteByIdAsync(string id)
        {
            return _userTheme.DeleteOneAsync(UserTheme => UserTheme.id == id);

        }

        public Task<UserTheme> getByUserIdAsync(string userId)
        {
            return _userTheme.Find<UserTheme>(userTheme => userTheme.userId == userId).FirstOrDefaultAsync();
        }
    }
}