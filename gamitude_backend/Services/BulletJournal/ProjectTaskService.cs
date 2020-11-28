using gamitude_backend.Models;
using MongoDB.Driver;
using gamitude_backend.Data;
using gamitude_backend.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;

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

        public ProjectTaskService(IDatabaseCollections dbCollections, IPageRepository pageRepository) : base(dbCollections)
        {
            _projectTasks = dbCollections.projectTasks;
            _pageRepository = pageRepository;
        }

        public async Task<List<ProjectTask>> getByJournalIdAndPageIdAsync(string userId, string journalId, string pageId)
        {
            var page = await _pageRepository.getByIdAsync(pageId);
            return await getActiveByDayOffsetAsync(userId,journalId, page.fromDay, page.toDay);
        }
    }
}