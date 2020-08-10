using gamitude_backend.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;
using AutoMapper;
using gamitude_backend.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace gamitude_backend.Services
{
    public interface IProjectService
    {
        Task<Project> getByIdAsync(int id);
        Task<List<Project>> getByUserIdAsync(String userId);
        Task<Project> createAsync(Project project);
        Task<Project> updateAsync(Project updateProject);
        Task deleteByIdAsync(int id);
    }
    public class ProjectService : IProjectService
    {
        private readonly ILogger<ProjectService> _logger;
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;

        public ProjectService(ILogger<ProjectService> logger,
            IMapper mapper,
            DataContext dbContext)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public Task<Project> getByIdAsync(int id)
        {
            return _dbContext.projects.AsNoTracking().FirstOrDefaultAsync(p => p.id == id);
        }

        public Task<List<Project>> getByUserIdAsync(String userId)
        {
            return _dbContext.projects.AsNoTracking().Where(p => p.userId == userId).ToListAsync();

        }


        public async Task<Project> createAsync(Project project)
        {
            await _dbContext.projects.AddAsync(project);
            await _dbContext.SaveChangesAsync();
            return project;
        }

        public async Task<Project> updateAsync(Project updateProject) 
        {
            var project = await _dbContext.projects.FirstOrDefaultAsync(p => p.id == updateProject.id);
            _mapper.Map<Project,Project>(updateProject,project);
            await _dbContext.SaveChangesAsync();
            return project;
        }

        public async Task deleteByIdAsync(int id)
        {
            var project = new Project{id=id};
            _dbContext.projects.Remove(project);
            await _dbContext.SaveChangesAsync();
        }

    }
}