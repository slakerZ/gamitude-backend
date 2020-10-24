using gamitude_backend.Models;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using gamitude_backend.Data;
using gamitude_backend.Repositories;

namespace gamitude_backend.Services
{
    public interface IProjectTaskService : IProjectTaskRepository
    {

    }
    public class ProjectTaskService : ProjectTaskRepository,IProjectTaskService
    {
        private readonly IMongoCollection<ProjectTask> _projectTasks;


        public ProjectTaskService(IDatabaseCollections dbCollections) : base(dbCollections)
        {
            _projectTasks = dbCollections.projectTasks;
        }

     
    }
}