using gamitude_backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using gamitude_backend.Data;

namespace gamitude_backend.Repositories
{
    public interface IProjectTaskRepository
    {
        Task<ProjectTask> getByIdAsync(string id);
        Task<List<ProjectTask>> getByProjectIdAsync(string userId);
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