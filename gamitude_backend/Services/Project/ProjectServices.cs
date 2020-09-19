using gamitude_backend.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using gamitude_backend.Settings;
using MongoDB.Driver;

namespace gamitude_backend.Services
{
    public interface IProjectService
    {
        Task<Project> getByIdAsync(String id);
        Task<List<Project>> getByUserIdAsync(String userId);
        Task createAsync(Project project);
        Task updateAsync(String id, Project updateProject);
        Task deleteByIdAsync(String id);
    }
    public class ProjectService : IProjectService
    {
        private readonly IMongoCollection<Project> _Projects;


        public ProjectService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.connectionString);
            var database = client.GetDatabase(settings.databaseName);

            _Projects = database.GetCollection<Project>(settings.projectsCollectionName);
        }

        public Task<Project> getByIdAsync(String id)
        {
            return _Projects.Find<Project>(Project => Project.Id == id).FirstOrDefaultAsync();
        }

        public Task<List<Project>> getByUserIdAsync(String userId)
        {
            return _Projects.Find<Project>(Project => Project.UserId == userId).ToListAsync();

        }

        public Task createAsync(Project Project)
        {
            return _Projects.InsertOneAsync(Project);
        }

        public Task updateAsync(String id, Project newProject)
        {
            return _Projects.ReplaceOneAsync(Project => Project.Id == id, newProject);

        }

        public Task deleteByProjectAsync(Project ProjectIn)
        {
            return _Projects.DeleteOneAsync(Project => Project.Id == ProjectIn.Id);
        }

        public Task deleteByIdAsync(string id)
        {
            return _Projects.DeleteOneAsync(Project => Project.Id == id);

        }

    }
}