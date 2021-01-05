using gamitude_backend.Models;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

using MongoDB.Driver;
using gamitude_backend.Data;
using gamitude_backend.Repositories;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace gamitude_backend.Services
{
    public interface IProjectLogService : IProjectLogRepository
    {
        Task<ProjectLog> processCreateProjectLog(ProjectLog projectLog);
        Task processDeleteProjectLog(string projectLogId, string userId);
    }
    public class ProjectLogService : ProjectLogRepository, IProjectLogService
    {
        private readonly IMongoCollection<ProjectLog> _projectLogs;
        private readonly ILogger<ProjectLogService> _logger;
        private readonly IStatsRepository _statsRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IDailyEnergyRepository _dailyEnergyRepository;

        public ProjectLogService(IDatabaseCollections dbCollections,
        ILogger<ProjectLogService> logger,
        IStatsRepository statsRepository,
        IProjectRepository projectRepository,
        IDailyEnergyRepository dailyEnergyRepository) : base(dbCollections)
        {
            _projectLogs = dbCollections.projectLogs;
            _logger = logger;
            _statsRepository = statsRepository;
            _projectRepository = projectRepository;
            _dailyEnergyRepository = dailyEnergyRepository;
        }

        private delegate long Del(long a, long b);

        private long add(long a, long b)
        {
            return a + b;
        }

        private long substract(long a, long b)
        {
            return a - b;
        }
        private STATS updateStatsTo(STATS stats)
        {
            switch (stats)
            {
                case STATS.BODY: return STATS.STRENGTH;
                case STATS.MIND: return STATS.INTELLIGENCE;
                case STATS.EMOTIONS: return STATS.CREATIVITY;
                case STATS.SOUL: return STATS.FLUENCY;
                default: return stats;
            }
        }
        private STATS updateStatsFrom(STATS stats)
        {
            switch (stats)
            {
                case STATS.STRENGTH: return STATS.BODY;
                case STATS.INTELLIGENCE: return STATS.MIND;
                case STATS.CREATIVITY: return STATS.EMOTIONS;
                case STATS.FLUENCY: return STATS.SOUL;
                default: return stats;
            }
        }
        public delegate STATS statsChange(STATS stats);
        private ProjectLog updateStatsFields(statsChange update, ProjectLog projectLog)
        {
            if (projectLog.project.projectType != PROJECT_TYPE.STAT)
            {
                projectLog.project.dominantStat = update(projectLog.project.dominantStat.Value);
                projectLog.project.stats = projectLog.project.stats.Select(o => update(o)).ToArray();
            }
            return projectLog;
        }
        public async Task<ProjectLog> processCreateProjectLog(ProjectLog projectLog)
        {
            projectLog.dateCreated = DateTime.UtcNow;

            //WORKAROUND PARSE ENERGIES TO STATS
            projectLog = updateStatsFields(updateStatsTo, projectLog);

            Dictionary<STATS, int> wages = projectLog.getWages();
            List<Task> processTasks = new List<Task>();
            processTasks.Add(Task.Run(() => manageEnergyAsync(add, wages, projectLog)));
            processTasks.Add(Task.Run(() => manageStatsAsync(add, wages, projectLog)));
            await Task.WhenAll(processTasks);

            //WORKAROUND REVERT
            projectLog = updateStatsFields(updateStatsFrom, projectLog);
            await createAsync(projectLog);
            if (projectLog.project != null)
            {
                await _projectRepository.updateTotalTimeAsync(projectLog.project.id, projectLog.timeSpend);
            }
            return projectLog;
        }

        public async Task processDeleteProjectLog(string projectLogId, string userId)
        {
            var projectLog = await getByIdAsync(projectLogId);
            if (projectLog.userId != userId)
            {
                throw new UnauthorizedAccessException("ProjectLog don't belong to you");
            }
            Dictionary<STATS, int> wages = projectLog.getWages();
            List<Task> processTasks = new List<Task>();
            processTasks.Add(Task.Run(() => manageEnergyAsync(substract, wages, projectLog)));
            processTasks.Add(Task.Run(() => manageStatsAsync(substract, wages, projectLog)));
            await Task.WhenAll(processTasks);
            await deleteByIdAsync(projectLogId);
            if (projectLog.project != null)
            {
                await _projectRepository.updateTotalTimeAsync(projectLog.project.id, projectLog.timeSpend * -1);
            }
        }

        private async Task manageEnergyAsync(Del op, Dictionary<STATS, int> wages, ProjectLog projectLog)
        {
            DailyEnergy dailyEnergy = await _dailyEnergyRepository.getByDateAndUserIdAsync(projectLog.dateCreated, projectLog.userId);
            if (dailyEnergy == null)
            {
                dailyEnergy = new DailyEnergy().init();
            }
            dailyEnergy.userId = projectLog.userId;
            dailyEnergy.dateCreated = projectLog.dateCreated.Date;
            dailyEnergy = calculateAndUpdateEnergy(op, dailyEnergy, wages, projectLog);
            if (dailyEnergy != null)
            {
                await _dailyEnergyRepository.createOrUpdateAsync(dailyEnergy);
            }
        }

        private async Task manageStatsAsync(Del op, Dictionary<STATS, int> wages, ProjectLog projectLog)
        {
            var stats = await _statsRepository.getByUserIdAsync(projectLog.userId);
            stats = calculateAndUpdateStats(op, stats, wages, projectLog);
            if (stats != null)
            {
                await _statsRepository.updateAsync(stats.id, stats);
            }

        }

        private DailyEnergy calculateAndUpdateEnergy(Del op, DailyEnergy dailyEnergy, Dictionary<STATS, int> wages, ProjectLog projectLog)
        {
            switch (projectLog.type)
            {
                case (PROJECT_TYPE.ENERGY):
                    return updateEnergiesWithWages(op, dailyEnergy, wages, projectLog.timeSpend >> 1);
                case (PROJECT_TYPE.STAT):
                    return updateEnergiesWithWages(op, dailyEnergy, wages, projectLog.timeSpend * -1);
                case (PROJECT_TYPE.BREAK):
                    return updateEnergiesWithWages(op, dailyEnergy, wages, projectLog.timeSpend >> 1);
                default:
                    return null;
            }

        }

        private DailyEnergy updateEnergiesWithWages(Del op, DailyEnergy dailyEnergy, Dictionary<STATS, int> wages, int duration)
        {
            int sum = wages.Sum(x => x.Value);
            dailyEnergy.body = (int)op(dailyEnergy.body, duration * wages.GetValueOrDefault(STATS.STRENGTH) / sum);
            dailyEnergy.soul = (int)op(dailyEnergy.soul, duration * wages.GetValueOrDefault(STATS.FLUENCY) / sum);
            dailyEnergy.emotions = (int)op(dailyEnergy.emotions, duration * wages.GetValueOrDefault(STATS.CREATIVITY) / sum);
            dailyEnergy.mind = (int)op(dailyEnergy.mind, duration * wages.GetValueOrDefault(STATS.INTELLIGENCE) / sum);
            return dailyEnergy;

        }

        private Stats calculateAndUpdateStats(Del op, Stats stats, Dictionary<STATS, int> wages, ProjectLog projectLog)
        {
            switch (projectLog.type)
            {
                case (PROJECT_TYPE.ENERGY):
                    return updateStatsWithWages(op, stats, wages, projectLog.timeSpend >> 2);
                case (PROJECT_TYPE.STAT):
                    return updateStatsWithWages(op, stats, wages, projectLog.timeSpend);
                case (PROJECT_TYPE.BREAK):
                    return updateStatsWithWages(op, stats, wages, 0);
                default:
                    return null;
            }
        }

        private Stats updateStatsWithWages(Del op, Stats stats, Dictionary<STATS, int> wages, int duration)
        {
            int sum = wages.Sum(x => x.Value);
            stats.strength = op(stats.strength, duration * wages.GetValueOrDefault(STATS.STRENGTH) / sum);
            stats.creativity = op(stats.creativity, duration * wages.GetValueOrDefault(STATS.CREATIVITY) / sum);
            stats.fluency = op(stats.fluency, duration * wages.GetValueOrDefault(STATS.FLUENCY) / sum);
            stats.intelligence = op(stats.intelligence, duration * wages.GetValueOrDefault(STATS.INTELLIGENCE) / sum);
            return stats;
        }

    }
}