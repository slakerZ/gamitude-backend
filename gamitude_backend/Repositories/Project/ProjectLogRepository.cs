using gamitude_backend.Models;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using gamitude_backend.Data;

namespace gamitude_backend.Repositories
{
    public interface IProjectLogRepository
    {
        Task<ProjectLog> getByIdAsync(String id);
        Task<List<ProjectLog>> getByUserIdAsync(String userId);
        System.Threading.Tasks.Task createAsync(ProjectLog projectLog);
        System.Threading.Tasks.Task updateAsync(String id, ProjectLog updateProjectLog);
        System.Threading.Tasks.Task deleteByIdAsync(String id);
    }
    public class ProjectLogRepository : IProjectLogRepository
    {
        private readonly IMongoCollection<ProjectLog> _ProjectLogs;


        public ProjectLogRepository(IDatabaseCollections dbCollections)
        {
            _ProjectLogs = dbCollections.projectLogs;
        }

        public Task<ProjectLog> getByIdAsync(String id)
        {
            return _ProjectLogs.Find<ProjectLog>(ProjectLog => ProjectLog.id == id).FirstOrDefaultAsync();
        }

        public Task<List<ProjectLog>> getByUserIdAsync(String userId)
        {
            return _ProjectLogs.Find<ProjectLog>(ProjectLog => ProjectLog.userId == userId).ToListAsync();
        }

        public Task<List<ProjectLog>> getByProjectIdAsync(String projectId)
        {
            return _ProjectLogs.Find<ProjectLog>(ProjectLog => ProjectLog.projectId == projectId).ToListAsync();
        }

        public System.Threading.Tasks.Task createAsync(ProjectLog ProjectLog)
        {
            return _ProjectLogs.InsertOneAsync(ProjectLog);
        }

        public System.Threading.Tasks.Task updateAsync(String id, ProjectLog newProjectLog)
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