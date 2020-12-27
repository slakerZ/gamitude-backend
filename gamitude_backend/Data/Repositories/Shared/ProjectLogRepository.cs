using gamitude_backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using gamitude_backend.Data;

namespace gamitude_backend.Repositories
{
    public interface IProjectLogRepository
    {
        Task<ProjectLog> getByIdAsync(string id);
        Task<List<ProjectLog>> getByUserIdAsync(string userId);
        System.Threading.Tasks.Task createAsync(ProjectLog projectLog);
        System.Threading.Tasks.Task updateAsync(string id, ProjectLog updateProjectLog);
        System.Threading.Tasks.Task deleteByIdAsync(string id);
    }
    public class ProjectLogRepository : IProjectLogRepository
    {
        private readonly IMongoCollection<ProjectLog> _ProjectLogs;


        public ProjectLogRepository(IDatabaseCollections dbCollections)
        {
            _ProjectLogs = dbCollections.projectLogs;
        }

        public Task<ProjectLog> getByIdAsync(string id)
        {
            return _ProjectLogs.Find<ProjectLog>(ProjectLog => ProjectLog.id == id).FirstOrDefaultAsync();
        }

        public Task<List<ProjectLog>> getByUserIdAsync(string userId)
        {
            return _ProjectLogs.Find<ProjectLog>(ProjectLog => ProjectLog.userId == userId).ToListAsync();
        }

        public Task<List<ProjectLog>> getByProjectIdAsync(string projectId)
        {
            return _ProjectLogs.Find<ProjectLog>(ProjectLog => ProjectLog.project.id == projectId).ToListAsync();
        }

        public System.Threading.Tasks.Task createAsync(ProjectLog ProjectLog)
        {
            return _ProjectLogs.InsertOneAsync(ProjectLog);
        }

        public System.Threading.Tasks.Task updateAsync(string id, ProjectLog newProjectLog)
        {
            return _ProjectLogs.ReplaceOneAsync(ProjectLog => ProjectLog.id == id, newProjectLog);

        }

        public System.Threading.Tasks.Task deleteByProjectLogAsync(ProjectLog ProjectLogIn)
        {
            return _ProjectLogs.DeleteOneAsync(ProjectLog => ProjectLog.id == ProjectLogIn.id);
        }

        public System.Threading.Tasks.Task deleteByIdAsync(string id)
        {
            return _ProjectLogs.DeleteOneAsync(ProjectLog => ProjectLog.id == id);

        }

    }
}