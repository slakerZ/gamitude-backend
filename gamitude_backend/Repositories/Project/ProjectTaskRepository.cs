using gamitude_backend.Models;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using gamitude_backend.Data;

namespace gamitude_backend.Repositories
{
    public interface IProjectTaskRepository
    {
        Task<ProjectTask> getByIdAsync(String id);
        Task<List<ProjectTask>> getByProjectIdAsync(String userId);
        Task createAsync(ProjectTask projectTask);
        Task updateAsync(String id, ProjectTask updateProjectTask);
        Task deleteByIdAsync(String id);
    }
    public class ProjectTaskRepository : IProjectTaskRepository
    {
        private readonly IMongoCollection<ProjectTask> _ProjectTasks;


        public ProjectTaskRepository(IDatabaseCollections dbCollections)
        {
            _ProjectTasks = dbCollections.projectTasks;
        }

        public Task<ProjectTask> getByIdAsync(String id)
        {
            return _ProjectTasks.Find<ProjectTask>(ProjectTask => ProjectTask.id == id).FirstOrDefaultAsync();
        }

        public Task<List<ProjectTask>> getByProjectIdAsync(String projectId)
        {
            return _ProjectTasks.Find<ProjectTask>(ProjectTask => ProjectTask.projectId == projectId).ToListAsync();

        }

        public Task createAsync(ProjectTask ProjectTask)
        {
            return _ProjectTasks.InsertOneAsync(ProjectTask);
        }

        public Task updateAsync(String id, ProjectTask newProjectTask)
        {
            return _ProjectTasks.ReplaceOneAsync(ProjectTask => ProjectTask.id == id, newProjectTask);

        }

        public Task deleteByProjectTaskAsync(ProjectTask ProjectTaskIn)
        {
            return _ProjectTasks.DeleteOneAsync(ProjectTask => ProjectTask.id == ProjectTaskIn.id);
        }

        public Task deleteByIdAsync(string id)
        {
            return _ProjectTasks.DeleteOneAsync(ProjectTask => ProjectTask.id == id);

        }

    }
}