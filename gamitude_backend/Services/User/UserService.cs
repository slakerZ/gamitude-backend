
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
using Microsoft.Extensions.Hosting;
using gamitude_backend.Configuration;
using SendGrid;
using System.Web;

namespace gamitude_backend.Services
{
    public interface IUserService
    {
        Task<User> getByIdAsync(string id);
        Task<long> getMoneyAsync(string userId);
        Task<User> createAsync(User newUser, string password);
        Task<User> getByUserNameAsync(string userName);
        Task changePasswordAsync(string id, string oldPassword, string newPassword);
        Task changeEmailAsync(string id, string newEmail);
        Task verifyEmail(string login, string token);
        Task<User> updateAsync(string userId, User updateUser);
        Task deleteByIdAsync(string id);
    }
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IDatabaseSettings _databaseSettings;
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
            // TODO add initializeUserTheme
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
            var emotionFolder = new Folder { userId = user.Id.ToString(), icon = "emotions", name = "Emotional Care", description = "Folder for projects that restores emotional energy" };
            var mindFolder = new Folder { userId = user.Id.ToString(), icon = "mind", name = "Nourish Mind ", description = "Folder for projects that restores mind energy" };
            var bodyFolder = new Folder { userId = user.Id.ToString(), icon = "body", name = "Relax Body", description = "Folder for projects that restores body energy" };
            var soulFolder = new Folder { userId = user.Id.ToString(), icon = "soul", name = "Feed Soul ", description = "Folder for projects that restores soul energy" };
            var creativityFolder = new Folder { userId = user.Id.ToString(), icon = "creativity", name = "Release Creativity", description = "Folder for projects that boosts creativity stats" };
            var intelligenceFolder = new Folder { userId = user.Id.ToString(), icon = "intelligence", name = "Develop Intelligence", description = "Folder for projects that boosts intelligence stats" };
            var strengthFolder = new Folder { userId = user.Id.ToString(), icon = "strength", name = "Strengthen Physique", description = "Folder for projects that boosts strength stats" };
            var fluencyFolder = new Folder { userId = user.Id.ToString(), icon = "fluency", name = "Learn Languages", description = "Folder for projects that boosts fluency stats" };

            var ultTimer = new Timer { userId = user.Id.ToString(), label = "90", timerType = TIMER_TYPE.TIMER, name = "90/30", countDownInfo = new CountDownInfo { breakTime = 30, overTime = 5, workTime = 90 } };
            var pomodoroTimer = new Timer { userId = user.Id.ToString(), label = "25", timerType = TIMER_TYPE.TIMER, name = "Pomodoro", countDownInfo = new CountDownInfo { breakTime = 5, overTime = 5, workTime = 25, breakInterval = 5, longerBreakTime = 15 } };
            var justFiveTimer = new Timer { userId = user.Id.ToString(), label = "5", timerType = TIMER_TYPE.TIMER, name = "Just Five", countDownInfo = new CountDownInfo { breakTime = 5, overTime = 5, workTime = 5 } };
            var stopwatchTimer = new Timer { userId = user.Id.ToString(), label = "SW", timerType = TIMER_TYPE.STOPWATCH, name = "Stopwatch", countDownInfo = null };

            List<Task> processTasks = new List<Task>
            {
                Task.Run(() =>  _folderRepository.createAsync(emotionFolder)),
                Task.Run(() =>  _folderRepository.createAsync(mindFolder)),
                Task.Run(() =>  _folderRepository.createAsync(bodyFolder)),
                Task.Run(() =>  _folderRepository.createAsync(soulFolder)),
                Task.Run(() =>  _folderRepository.createAsync(creativityFolder)),
                Task.Run(() =>  _folderRepository.createAsync(intelligenceFolder)),
                Task.Run(() =>  _folderRepository.createAsync(strengthFolder)),
                Task.Run(() =>  _folderRepository.createAsync(fluencyFolder)),
                Task.Run(() =>   _timerRepository.createAsync(ultTimer)),
                Task.Run(() => _timerRepository.createAsync(pomodoroTimer)),
                Task.Run(() => _timerRepository.createAsync(justFiveTimer)),
                Task.Run(() => _timerRepository.createAsync(stopwatchTimer))
            };
            await Task.WhenAll(processTasks);


            processTasks = new List<Task>
            {
                //ENERGY
                Task.Run(() =>  _projectRepository.createAsync(new Project{defaultTimerId = justFiveTimer.id, dateCreated = DateTime.UtcNow,name="Pranayama", folderId=emotionFolder.id,totalTimeSpend=0,dominantStat=STATS.EMOTIONS,stats = new STATS[] {STATS.EMOTIONS},projectType=PROJECT_TYPE.ENERGY,userId=user.Id.ToString(),timeSpendBreak=0 })),
                Task.Run(() =>  _projectRepository.createAsync(new Project{defaultTimerId = justFiveTimer.id, dateCreated = DateTime.UtcNow,name="Meditation", folderId=mindFolder.id,totalTimeSpend=0,dominantStat=STATS.MIND,stats = new STATS[] {STATS.MIND},projectType=PROJECT_TYPE.ENERGY,userId=user.Id.ToString(),timeSpendBreak=0 })),
                Task.Run(() =>  _projectRepository.createAsync(new Project{defaultTimerId = justFiveTimer.id, dateCreated = DateTime.UtcNow,name="Stretching", folderId=bodyFolder.id,totalTimeSpend=0,dominantStat=STATS.BODY,stats = new STATS[] {STATS.BODY},projectType=PROJECT_TYPE.ENERGY,userId=user.Id.ToString(),timeSpendBreak=0 })),
                Task.Run(() =>  _projectRepository.createAsync(new Project{defaultTimerId = stopwatchTimer.id, dateCreated = DateTime.UtcNow,name="Family Time", folderId=soulFolder.id,totalTimeSpend=0,dominantStat=STATS.SOUL,stats = new STATS[] {STATS.SOUL},projectType=PROJECT_TYPE.ENERGY,userId=user.Id.ToString(),timeSpendBreak=0 })),
                //STATS
                Task.Run(() =>  _projectRepository.createAsync(new Project{defaultTimerId = stopwatchTimer.id, dateCreated = DateTime.UtcNow,name="Painting", folderId=creativityFolder.id,totalTimeSpend=0,dominantStat=STATS.CREATIVITY ,stats = new STATS[] {STATS.CREATIVITY},projectType=PROJECT_TYPE.STAT,userId=user.Id.ToString(),timeSpendBreak=0 })),
                Task.Run(() =>  _projectRepository.createAsync(new Project{defaultTimerId = ultTimer.id, dateCreated = DateTime.UtcNow,name="Programming", folderId=intelligenceFolder.id,totalTimeSpend=0,dominantStat=STATS.INTELLIGENCE ,stats = new STATS[] {STATS.INTELLIGENCE},projectType=PROJECT_TYPE.STAT,userId=user.Id.ToString(),timeSpendBreak=0 })),
                Task.Run(() =>  _projectRepository.createAsync(new Project{defaultTimerId = justFiveTimer.id, dateCreated = DateTime.UtcNow,name="Push Ups", folderId=strengthFolder.id,totalTimeSpend=0,dominantStat=STATS.STRENGTH ,stats = new STATS[] {STATS.STRENGTH},projectType=PROJECT_TYPE.STAT,userId=user.Id.ToString(),timeSpendBreak=0 })),
                Task.Run(() =>  _projectRepository.createAsync(new Project{defaultTimerId = pomodoroTimer.id, dateCreated = DateTime.UtcNow,name="Learn Polish", folderId=fluencyFolder.id,totalTimeSpend=0,dominantStat=STATS.FLUENCY,stats = new STATS[] {STATS.FLUENCY},projectType=PROJECT_TYPE.STAT,userId=user.Id.ToString(),timeSpendBreak=0 }))
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

                if (!StaticValues.IsDevelopment())
                {
                    var response = await sendVerificationEmail(newUser.Email);
                    _logger.LogInformation(response.Headers.ToString() + response.StatusCode.ToString());
                }
                return newUser;
            }
            else
            {
                var s = result.Errors.AsEnumerable();
                throw new IdentityException(s);
            }
        }

        private async Task<Response> sendVerificationEmail(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = HttpUtility.UrlEncode(token);
            var link = $"https://gamitude.rocks/verifyEmail/{user.UserName}/{token}";
            return await _emailSender.SendVerificationEmailAsync(user.Email, user.UserName, link);
        }

        public async Task verifyEmail(string login, string token)
        {
            var user = await _userManager.FindByNameAsync(login);
            token = HttpUtility.UrlDecode(token);
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                var s = result.Errors.AsEnumerable();
                throw new IdentityException(s);
            }
        }
        public async Task verifyNewEmail(string login, string newEmail, string token)
        {
            var user = await _userManager.FindByNameAsync(login);
            token = HttpUtility.UrlDecode(token);
            var result = await _userManager.ChangeEmailAsync(user, newEmail, token);
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

        public async Task changeEmailAsync(string id, string newEmail)
        {
            // ! TODO CREATE NEW TEMPLATE EMAIL FOR CHANGE EMAIL
            var user = await _userManager.FindByIdAsync(id);
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
            token = HttpUtility.UrlEncode(token);
            var link = $"https://gamitude.rocks/verifyEmail/{user.UserName}/newEmail/{newEmail}/{token}";
            await _emailSender.SendVerificationEmailAsync(newEmail, user.UserName, link);
        }
    }

}