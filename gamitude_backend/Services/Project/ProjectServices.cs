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
    public interface IProjectService : IProjectRepository
    {

    }
    public class ProjectService : ProjectRepository, IProjectService
    {
        private readonly IMongoCollection<Project> _Projects;


        public ProjectService(IDatabaseCollections dbCollections) : base(dbCollections)
        {
            _Projects = dbCollections.projects;
        }


    }
}