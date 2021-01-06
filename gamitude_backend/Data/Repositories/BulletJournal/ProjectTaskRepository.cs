using gamitude_backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using gamitude_backend.Data;
using MongoDB.Driver.Linq;
using System;

namespace gamitude_backend.Repositories
{
    public interface IProjectTaskRepository
    {
        Task<ProjectTask> getByIdAsync(string id);
        Task<List<ProjectTask>> getByProjectIdAsync(string projectId);
        Task<List<ProjectTask>> getByUserIdAsync(string userId);
        Task<List<ProjectTask>> getActiveByDayOffsetAsync(string userId, string journalId, int fromDays, int toDays);
        Task<List<ProjectTask>> getOverdueAsync(string userId,string journalId);
        Task<List<ProjectTask>> getUnScheduledAsync(string userId,string journalId);
        Task createAsync(ProjectTask projectTask);
        Task updateAsync(string id, ProjectTask updateProjectTask);
        Task deleteByIdAsync(string id);
    }
    public class ProjectTaskRepository : IProjectTaskRepository
    {
        private readonly IMongoCollection<ProjectTask> _projectTasks;


        public ProjectTaskRepository(IDatabaseCollections dbCollections)
        {
            _projectTasks = dbCollections.projectTasks;
        }

        public Task<ProjectTask> getByIdAsync(string id)
        {
            return _projectTasks.Find<ProjectTask>(ProjectTask => ProjectTask.id == id).FirstOrDefaultAsync();
        }

        public Task<List<ProjectTask>> getByUserIdAsync(string userId)
        {
            return _projectTasks.Find<ProjectTask>(ProjectTask => ProjectTask.userId == userId).ToListAsync();
        }

        //TODO unit test
        public Task<List<ProjectTask>> getActiveByDayOffsetAsync(string userId, string journalId, int fromDays, int toDays)
        {
            var query = _projectTasks.AsQueryable()
                .Where(o => o.journalId == journalId)
                .Where(o => o.dateFinished == null)
                .Where(o => o.userId == userId)
                .Where(o => o.deadLine >= DateTime.UtcNow.Date.AddDays(fromDays));
            if(toDays != 0) // if 0 means forever
            {
                query = query.Where(o => o.deadLine < DateTime.UtcNow.Date.AddDays(toDays));
            }

            var projectTasks = query.ToListAsync();
            return projectTasks;
        }

        public Task<List<ProjectTask>> getOverdueAsync(string userId,string journalId)
        {
            var projectTasks = _projectTasks.AsQueryable()
                .Where(o => o.journalId == journalId)
                .Where(o => o.dateFinished == null)
                .Where(o => o.userId == userId)
                .Where(o => o.deadLine < DateTime.UtcNow.Date)
                .ToListAsync();
            return projectTasks;
        }

        public Task<List<ProjectTask>> getUnScheduledAsync(string userId,string journalId)
        {
            var projectTasks = _projectTasks.AsQueryable()
                .Where(o => o.journalId == journalId)
                .Where(o => o.userId == userId)
                .Where(o => o.dateFinished == null)
                .Where(o => o.deadLine == null )
                .ToListAsync();
            return projectTasks;
        }

        public Task<List<ProjectTask>> getByProjectIdAsync(string projectId)
        {
            return _projectTasks.Find<ProjectTask>(ProjectTask => ProjectTask.projectId == projectId).ToListAsync();
        }

        public Task createAsync(ProjectTask ProjectTask)
        {
            return _projectTasks.InsertOneAsync(ProjectTask);
        }

        public Task updateAsync(string id, ProjectTask newProjectTask)
        {
            return _projectTasks.ReplaceOneAsync(ProjectTask => ProjectTask.id == id, newProjectTask);
        }

        public Task deleteByProjectTaskAsync(ProjectTask ProjectTaskIn)
        {
            return _projectTasks.DeleteOneAsync(ProjectTask => ProjectTask.id == ProjectTaskIn.id);
        }

        public Task deleteByIdAsync(string id)
        {
            return _projectTasks.DeleteOneAsync(ProjectTask => ProjectTask.id == id);
        }

    }
}