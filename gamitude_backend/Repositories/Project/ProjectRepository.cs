using gamitude_backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using gamitude_backend.Data;
using MongoDB.Bson;

namespace gamitude_backend.Repositories
{
    public interface IProjectRepository
    {
        Task<Project> getByIdAsync(string id);
        Task<List<Project>> getByUserIdAsync(string userId);
        Task createAsync(Project project);
        Task updateAsync(string id, Project updateProject);
        Task updateTotalTimeAsync(string projectId, int timeToAdd);
        Task updateTotalTimeBreakAsync(string projectId, int timeToAdd);
        Task deleteByIdAsync(string id);
    }
    public class ProjectRepository : IProjectRepository
    {
        private readonly IMongoCollection<Project> _Projects;


        public ProjectRepository(IDatabaseCollections dbCollections)
        {
            _Projects = dbCollections.projects;
        }

        public Task<Project> getByIdAsync(string id)
        {
            return _Projects.Find<Project>(Project => Project.id == id).FirstOrDefaultAsync();
        }

        public Task<List<Project>> getByUserIdAsync(string userId)
        {
            return _Projects.Find<Project>(Project => Project.userId == userId).ToListAsync();

        }

        public Task createAsync(Project Project)
        {
            return _Projects.InsertOneAsync(Project);
        }

        public Task updateAsync(string id, Project newProject)
        {
            return _Projects.ReplaceOneAsync(Project => Project.id == id, newProject);

        }
        public Task updateTotalTimeAsync(string projectId, int timeToAdd)
        {
            var filter = new BsonDocument("_id", new ObjectId(projectId));
            var update = new BsonDocument("$inc", new BsonDocument("totalTimeSpend", timeToAdd));
            return _Projects.UpdateOneAsync(filter,update);
        }
        public Task updateTotalTimeBreakAsync(string projectId, int timeToAdd)
        {
            var filter = new BsonDocument("_id", new ObjectId(projectId));
            var update = new BsonDocument("$inc", new BsonDocument("totalTimeSpendBreak", timeToAdd));
            return _Projects.UpdateOneAsync(filter,update);
        }


        public Task deleteByProjectAsync(Project ProjectIn)
        {
            return _Projects.DeleteOneAsync(Project => Project.id == ProjectIn.id);
        }

        public Task deleteByIdAsync(string id)
        {
            return _Projects.DeleteOneAsync(Project => Project.id == id);

        }

    }
}