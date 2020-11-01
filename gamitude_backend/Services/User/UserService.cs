
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using gamitude_backend.Dto.Authorization;
using gamitude_backend.Dto.User;
using gamitude_backend.Exceptions;
using gamitude_backend.Settings;
using gamitude_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using gamitude_backend.Repositories;
using MongoDB.Driver;

namespace gamitude_backend.Services
{
    public interface IUserService
    {
        Task<List<User>> getAllAsync(int offset, int limit);
        Task<User> getByIdAsync(String id);
        Task<User> createAsync(User newUser, String password);
        Task<User> getByUserNameAsync(String userName);
        Task changePasswordAsync(String id, String oldPassword, String newPassword);
        Task<User> updateAsync(User updateUser);
        Task deleteByIdAsync(String id);
    }
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IDatabaseSettings _databaseSettings;

        //TODO make async
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IRankRepository _rankRepository;
        private readonly IUserRankRepository _userRankRepository;
        private readonly IStatsRepository _statsRepository;
        private readonly IFolderRepository _folderRepository;
        private readonly ITimerRepository _timerRepository;
        private readonly IUserRanksRepository _userRanksRepository;

        public UserService(ILogger<UserService> logger,
        IDatabaseSettings databaseSettings,
         UserManager<User> userManager,
          IMapper mapper,
          IRankRepository rankRepository,
          IUserRankRepository userRankRepository,
          IStatsRepository statsRepository,
          IFolderRepository folderRepository,
          ITimerRepository timerRepository,
          IUserRanksRepository userRanksRepository)
        {
            _logger = logger;
            _databaseSettings = databaseSettings;
            _userManager = userManager;
            _mapper = mapper;
            _rankRepository = rankRepository;
            _userRankRepository = userRankRepository;
            _statsRepository = statsRepository;
            _folderRepository = folderRepository;
            _timerRepository = timerRepository;
            _userRanksRepository = userRanksRepository;
        }
        private Task initializeUser(User user)
        {
            List<Task> processTasks = new List<Task>();
            processTasks.Add(Task.Run(() => initializeUserRank(user)));
            processTasks.Add(Task.Run(() => initializeUserStats(user)));
            processTasks.Add(Task.Run(() => initializeUserFolder(user)));
            processTasks.Add(Task.Run(() => initializeUserTimer(user)));
            //TODO add initializeUserTheme
            return Task.WhenAll(processTasks);
        }
        private async Task initializeUserRank(User user)
        {
            await _statsRepository.createAsync(new Stats
            {
                userId = user.Id.ToString()
            });
        }
        private Task initializeUserTimer(User user)
        {
            List<Task> processTasks = new List<Task>();
            processTasks.Add(Task.Run(() => _timerRepository.createAsync(new Timer{userId=user.Id.ToString(), name = "90/30",breakTime=30,overTime=5,workTime=90})));
            processTasks.Add(Task.Run(() => _timerRepository.createAsync(new Timer{userId=user.Id.ToString(), name = "Pomodoro",breakTime=5,overTime=5,workTime=25})));
            processTasks.Add(Task.Run(() => _timerRepository.createAsync(new Timer{userId=user.Id.ToString(), name = "Just Five",breakTime=5,overTime=5,workTime=5})));
            
            return Task.WhenAll(processTasks);

        }

        private Task initializeUserFolder(User user)
        {
            List<Task> processTasks = new List<Task>();
            processTasks.Add(Task.Run(() => _folderRepository.createAsync(new Folder{userId=user.Id.ToString(), name = "Active"})));
            processTasks.Add(Task.Run(() => _folderRepository.createAsync(new Folder{userId=user.Id.ToString(), name = "Inactive "})));
            processTasks.Add(Task.Run(() => _folderRepository.createAsync(new Folder{userId=user.Id.ToString(), name = "Done"})));
            
            return Task.WhenAll(processTasks);

        }

        private async Task initializeUserStats(User user)
        {
            var rookieRank = await _rankRepository.getRookieAsync();
            var userRanks = new UserRanks { userId = user.Id.ToString() };
            userRanks.rankIds.Add(rookieRank.id);
            await _userRanksRepository.createAsync(userRanks);
            await _userRankRepository.createOrUpdateAsync(new UserRank
            {
                userId = user.Id.ToString(),
                rankId = rookieRank.id
            });
        }
        public async Task<User> createAsync(User newUser, String password)
        {
            var result = await _userManager.CreateAsync(newUser, password);
            if (result.Succeeded)
            {
                await initializeUser(newUser);
                return newUser;
            }
            else
            {
                var s = result.Errors.AsEnumerable();
                throw new IdentityException(s);
            }
        }

        public async Task<List<User>> getAllAsync(int offset, int limit)
        {
            var users = await _userManager.Users
                .OrderByDescending(o => o.Id)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
            return users;
        }

        public Task<User> getByIdAsync(String id)
        {
            return _userManager.FindByIdAsync(id);
        }

        public Task<User> getByUserNameAsync(String userName)
        {
            return _userManager.FindByNameAsync(userName);
        }

        public async Task<User> updateAsync(User updateUser)
        {
            var user = await _userManager.FindByIdAsync(updateUser.Id.ToString());
            user = _mapper.Map<User, User>(updateUser, user);
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return user;
            }
            else
            {
                var s = result.Errors.AsEnumerable();
                throw new IdentityException(s);
            }
        }
        public async Task changePasswordAsync(String id, String oldPassword, String newPassword)
        {
            var user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!result.Succeeded)
            {
                var s = result.Errors.AsEnumerable();
                throw new IdentityException(s);
            }
        }

        public async Task deleteByIdAsync(String id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var s = result.Errors.AsEnumerable();
                throw new IdentityException(s);
            }
        }
    }

}