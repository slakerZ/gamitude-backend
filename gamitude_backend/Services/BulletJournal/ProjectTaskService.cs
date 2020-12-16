using gamitude_backend.Models;
using MongoDB.Driver;
using gamitude_backend.Data;
using gamitude_backend.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace gamitude_backend.Services
{
    public interface IProjectTaskService : IProjectTaskRepository
    {
        Task<List<ProjectTask>> getByJournalIdAndPageIdAsync(string userId, string journalId, string pageId);
    }
    public class ProjectTaskService : ProjectTaskRepository, IProjectTaskService
    {
        private readonly IMongoCollection<ProjectTask> _projectTasks;
        private readonly IPageRepository _pageRepository;
        private readonly ILogger<ProjectTaskService> _logger;

        public ProjectTaskService(IDatabaseCollections dbCollections, IPageRepository pageRepository,ILogger<ProjectTaskService> logger) : base(dbCollections)
        {
            _projectTasks = dbCollections.projectTasks;
            _pageRepository = pageRepository;
            _logger = logger;
        }

        public async Task<List<ProjectTask>> getByJournalIdAndPageIdAsync(string userId, string journalId, string pageId)
        {
            var page = await _pageRepository.getByIdAsync(pageId);
            _logger.LogDebug(page.pageType.ToString());
            List<ProjectTask> projectTasks = null;
            switch (page.pageType)
            {
                case PAGE_TYPE.NORMAL: projectTasks = await getActiveByDayOffsetAsync(userId, journalId, page.beetwenDays.fromDay, page.beetwenDays.toDay); break;
                case PAGE_TYPE.OVERDUE: projectTasks = await getOverdueAsync(userId,journalId); break;
                case PAGE_TYPE.UNSCHEDULED: projectTasks = await getUnScheduledAsync(userId,journalId); break;
            }
            return projectTasks;
        }

    }
}