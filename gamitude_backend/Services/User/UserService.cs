
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using gamitude_backend.Exceptions;
using gamitude_backend.Settings;
using gamitude_backend.Models;
using Microsoft.AspNetCore.Identity;

using Microsoft.Extensions.Logging;
using gamitude_backend.Repositories;
using MongoDB.Driver;

namespace gamitude_backend.Services
{
    public interface IUserService
    {
        Task<User> getByIdAsync(string id);
        Task<User> createAsync(User newUser, string password);
        Task<User> getByUserNameAsync(string userName);
        Task changePasswordAsync(string id, string oldPassword, string newPassword);
        Task<User> updateAsync(User updateUser);
        Task deleteByIdAsync(string id);
    }
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IDatabaseSettings _databaseSettings;

        //TODO make async
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IRankRepository _rankRepository;
        private readonly IStatsRepository _statsRepository;
        private readonly IFolderRepository _folderRepository;
        private readonly ITimerRepository _timerRepository;

        public UserService(ILogger<UserService> logger,
        IDatabaseSettings databaseSettings,
         UserManager<User> userManager,
          IMapper mapper,
          IRankRepository rankRepository,
          IStatsRepository statsRepository,
          IFolderRepository folderRepository,
          ITimerRepository timerRepository)
        {
            _logger = logger;
            _databaseSettings = databaseSettings;
            _userManager = userManager;
            _mapper = mapper;
            _rankRepository = rankRepository;
            _statsRepository = statsRepository;
            _folderRepository = folderRepository;
            _timerRepository = timerRepository;
        }

        private Task initializeUser(User user)
        {
            List<Task> processTasks = new List<Task>
            {
                Task.Run(() => initializeUserStats(user)),
                Task.Run(() => initializeUserFolder(user)),
                Task.Run(() => initializeUserTimer(user))
            };
            //TODO add initializeUserTheme
            return Task.WhenAll(processTasks);
        }

        private async Task initializeUserStats(User user)
        {
            await _statsRepository.createAsync(new Stats
            {
                userId = user.Id.ToString()
            });
        }

        private Task initializeUserTimer(User user)
        {
            List<Task> processTasks = new List<Task>
            {
                Task.Run(() => _timerRepository.createAsync(new Timer { userId = user.Id.ToString(),label="90" , timerType=TIMER_TYPE.TIMER, name = "90/30", breakTime = 30, overTime = 5, workTime = 90 })),
                Task.Run(() => _timerRepository.createAsync(new Timer { userId = user.Id.ToString(),label="25" , timerType=TIMER_TYPE.TIMER, name = "Pomodoro", breakTime = 5, overTime = 5, workTime = 25 })),
                Task.Run(() => _timerRepository.createAsync(new Timer { userId = user.Id.ToString(),label="5" , timerType=TIMER_TYPE.TIMER, name = "Just Five", breakTime = 5, overTime = 5, workTime = 5 }))
            };

            return Task.WhenAll(processTasks);

        }

        private Task initializeUserFolder(User user)
        {
            List<Task> processTasks = new List<Task>
            {
                Task.Run(() => _folderRepository.createAsync(new Folder { userId = user.Id.ToString(),icon="active", name = "Active", description = "Folder for active projects" })),
                Task.Run(() => _folderRepository.createAsync(new Folder { userId = user.Id.ToString(),icon="paused", name = "Inactive", description = "Folder for inactive projects" })),
                Task.Run(() => _folderRepository.createAsync(new Folder { userId = user.Id.ToString(),icon="done", name = "Done", description = "Folder for finished projects" }))
            };

            return Task.WhenAll(processTasks);

        }

        private async Task<User> initializeUserRank(User user)
        {
            var rookieRank = await _rankRepository.getRookieAsync();
            user.currentRankId = rookieRank.id;
            user.purchasedRankIds.Add(rookieRank.id);
            return user;
        }

        public async Task<User> createAsync(User newUser, string password)
        {
            newUser = await initializeUserRank(newUser);
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

        public Task<User> getByIdAsync(string id)
        {
            return _userManager.FindByIdAsync(id);
        }

        public Task<User> getByUserNameAsync(string userName)
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

        public async Task changePasswordAsync(string id, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!result.Succeeded)
            {
                var s = result.Errors.AsEnumerable();
                throw new IdentityException(s);
            }
        }

        public async Task deleteByIdAsync(string id)
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