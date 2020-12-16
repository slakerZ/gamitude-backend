
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
        Task<long> getMoneyAsync(string userId);
        Task<User> createAsync(User newUser, string password);
        Task<User> getByUserNameAsync(string userName);
        Task changePasswordAsync(string id, string oldPassword, string newPassword);
        Task<User> updateAsync(string userId, User updateUser);
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
        private readonly IJournalRepository _journalRepository;
        private readonly IPageRepository _pageRepository;
        private readonly IMoneyRepository _moneyRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IEmailSender _emailSender;

        public UserService(ILogger<UserService> logger,
        IDatabaseSettings databaseSettings,
         UserManager<User> userManager,
          IMapper mapper,
          IRankRepository rankRepository,
          IStatsRepository statsRepository,
          IFolderRepository folderRepository,
          ITimerRepository timerRepository,
          IJournalRepository journalRepository,
          IPageRepository pageRepository,
          IMoneyRepository moneyRepository,
          IProjectRepository projectRepository,
          IEmailSender emailSender)
        {
            _logger = logger;
            _databaseSettings = databaseSettings;
            _userManager = userManager;
            _mapper = mapper;
            _rankRepository = rankRepository;
            _statsRepository = statsRepository;
            _folderRepository = folderRepository;
            _timerRepository = timerRepository;
            _journalRepository = journalRepository;
            _pageRepository = pageRepository;
            _moneyRepository = moneyRepository;
            _projectRepository = projectRepository;
            _emailSender = emailSender;
        }

        private Task initializeUser(User user)
        {
            List<Task> processTasks = new List<Task>
            {
                Task.Run(() => initializeUserStats(user)),
                Task.Run(() => initializeUserTimersFoldersAndProjects(user)),
                Task.Run(() => initializeUserJournals(user))

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

        private async Task initializeUserTimersFoldersAndProjects(User user)
        {
            var demoTimer = new Timer { userId = user.Id.ToString(), label = "90", timerType = TIMER_TYPE.TIMER, name = "90/30", countDownInfo = new CountDownInfo { breakTime = 30, overTime = 5, workTime = 90 } };
            var demoFolder = new Folder { userId = user.Id.ToString(), icon = "active", name = "Active", description = "Folder for active projects" };

            await _folderRepository.createAsync(demoFolder);
            await _timerRepository.createAsync(demoTimer);

            List<Task> processTasks = new List<Task>
            {
                Task.Run(() => _timerRepository.createAsync(new Timer { userId = user.Id.ToString(),label="25" , timerType=TIMER_TYPE.TIMER, name = "Pomodoro",countDownInfo = new CountDownInfo{ breakTime = 5, overTime = 5, workTime = 25, breakInterval=5, longerBreakTime=15} })),
                Task.Run(() => _timerRepository.createAsync(new Timer { userId = user.Id.ToString(),label="5" , timerType=TIMER_TYPE.TIMER, name = "Just Five",countDownInfo = new CountDownInfo{ breakTime = 5, overTime = 5, workTime = 5} })),
                Task.Run(() => _timerRepository.createAsync(new Timer { userId = user.Id.ToString(),label="SW" , timerType=TIMER_TYPE.STOPWATCH, name = "Stopwatch",countDownInfo = null })),

                Task.Run(() => _folderRepository.createAsync(new Folder { userId = user.Id.ToString(),icon="paused", name = "Inactive", description = "Folder for inactive projects" })),
                Task.Run(() => _folderRepository.createAsync(new Folder { userId = user.Id.ToString(),icon="done", name = "Done", description = "Folder for finished projects" })),

                Task.Run(() =>  _projectRepository.createAsync(new Project{defaultTimerId = demoTimer.id, dateCreated = DateTime.UtcNow,name="Engineering thesis",folderId=demoFolder.id,totalTimeSpend=0,dominantStat=STATS.INTELLIGENCE,stats = new STATS[] {STATS.INTELLIGENCE},projectType=PROJECT_TYPE.STAT,userId=user.Id.ToString(),timeSpendBreak=0 })),
                Task.Run(() =>  _projectRepository.createAsync(new Project{defaultTimerId = demoTimer.id, dateCreated = DateTime.UtcNow,name="Recreational book reading",folderId=demoFolder.id,totalTimeSpend=0,dominantStat=STATS.MIND,stats = new STATS[] {STATS.MIND},projectType=PROJECT_TYPE.ENERGY,userId=user.Id.ToString(),timeSpendBreak=0 })),
            };
            await Task.WhenAll(processTasks);
        }

        private async Task initializeUserJournals(User user)
        {
            var journal = new Journal { userId = user.Id.ToString(), dateCreated = DateTime.UtcNow, description = "Your first journal", icon = "active", name = "Life" };
            await _journalRepository.createAsync(journal);
            List<Task> processTasks = new List<Task>
            {
                Task.Run(() => _pageRepository.createAsync(new Page {dateCreated = DateTime.UtcNow,journalId=journal.id,pageType=PAGE_TYPE.NORMAL,beetwenDays= new BeetwenDays{ fromDay=0, toDay=1},userId = user.Id.ToString(),icon="active", name = "Today", description = "Folder for tasks for today" })),
                Task.Run(() => _pageRepository.createAsync(new Page {dateCreated = DateTime.UtcNow,journalId=journal.id,pageType=PAGE_TYPE.NORMAL,beetwenDays= new BeetwenDays{ fromDay=1, toDay=8}, userId = user.Id.ToString(),icon="active", name = "Week", description = "Folder for tasks for this week" })),
                Task.Run(() => _pageRepository.createAsync(new Page {dateCreated = DateTime.UtcNow,journalId=journal.id,pageType=PAGE_TYPE.NORMAL,beetwenDays= new BeetwenDays{ fromDay=8, toDay=0} ,userId = user.Id.ToString(),icon="active", name = "Scheduled Future", description = "Page with all scheduled task that are further than week from now" })),
                Task.Run(() => _pageRepository.createAsync(new Page {dateCreated = DateTime.UtcNow,journalId=journal.id,pageType=PAGE_TYPE.OVERDUE, userId = user.Id.ToString(),icon="done", name = "Overdue", description = "Folder for tasks after deadline" })),
                Task.Run(() => _pageRepository.createAsync(new Page {dateCreated = DateTime.UtcNow,journalId=journal.id,pageType=PAGE_TYPE.UNSCHEDULED, userId = user.Id.ToString(),icon="paused", name = "Unscheduled", description = "Page with all unscheduled tasks that are not finished" }))
            };
            await Task.WhenAll(processTasks);
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
                await sendVerificationEmail(newUser.Email);
                return newUser;
            }
            else
            {
                var s = result.Errors.AsEnumerable();
                throw new IdentityException(s);
            }
        }

        private async Task sendVerificationEmail(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _emailSender.SendVerificationEmailAsync(user.Email, user.UserName, token);
        }

        public async Task verifyEmail(string userEmail, string token)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
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

        public async Task<User> updateAsync(string userId, User updateUser)
        {
            var user = await _userManager.FindByIdAsync(userId);
            user = _mapper.Map<User, User>(updateUser, user);
            _logger.LogInformation(user.UserName);
            _logger.LogInformation(user.Email);
            _logger.LogInformation(user.Id.ToString());
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

        public Task<long> getMoneyAsync(string userId)
        {
            return _moneyRepository.getMoneyByUserIdAsync(userId);
        }
    }

}