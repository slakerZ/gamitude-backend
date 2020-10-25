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
        Task<ProjectLog> processCreateProjectLog(ProjectLog projectLog);
        Task processDeleteProjectLog(String projectLogId);
    }
    public class ProjectLogService : ProjectLogRepository, IProjectLogService
    {
        private readonly IMongoCollection<ProjectLog> _projectLogs;


        public ProjectLogService(IDatabaseCollections dbCollections) : base(dbCollections)
        {
            _projectLogs = dbCollections.projectLogs;
        }

        public Task<ProjectLog> processCreateProjectLog(ProjectLog projectLog)
        {
            throw new NotImplementedException();
        }

        public Task processDeleteProjectLog(string projectLogId)
        {
            throw new NotImplementedException();
        }
    }
}