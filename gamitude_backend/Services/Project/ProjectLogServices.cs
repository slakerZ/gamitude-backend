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
    public interface IProjectLogService : IProjectLogRepository
    {

    }
    public class ProjectLogService : ProjectLogRepository, IProjectLogService
    {
        private readonly IMongoCollection<ProjectLog> _projectLogs;


        public ProjectLogService(IDatabaseCollections dbCollections) : base(dbCollections)
        {
            _projectLogs = dbCollections.projectLogs;
        }

    }
}