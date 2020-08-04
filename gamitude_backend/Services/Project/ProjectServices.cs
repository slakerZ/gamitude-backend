// using gamitude_backend.Models;
// using System.Collections.Generic;
// using System.Linq;
// using System;
// using Microsoft.Extensions.Logging;
// using AutoMapper;
// using gamitude_backend.Data;

// namespace gamitude_backend.Services
// {

//     //Add interface 
//     public class ProjectService
//     {

//         public ILogger<ProjectService> _logger { get; }
//         public IMapper _mapper { get; }
//         public DataContext _dbContext { get; }

//         public ProjectService(ILogger<ProjectService> logger,
//             IMapper mapper,
//             DataContext dbContext)
//         {
//             _logger = logger;
//             _mapper = mapper;
//             _dbContext = dbContext;
//         }

//         // public List<Project> GetProjectsByUserId(string userId) =>
//         //     _Projects.Find<Project>(Project => Project.UserId == userId).ToList();

//         // public Project Get(string id) =>
//         //     _Projects.Find<Project>(Project => Project.Id == id).FirstOrDefault();

//         // public Project Create(Project Project)
//         // {
//         //     _Projects.InsertOne(Project);
//         //     return Project;
//         // }

//         // public void Update(string id, Project newProject) =>
//         //     _Projects.ReplaceOne(Project => Project.Id == id, newProject);

//         // public void Remove(Project ProjectIn) =>
//         //     _Projects.DeleteOne(Project => Project.Id == ProjectIn.Id);

//         // public void Remove(string id) =>
//         //     _Projects.DeleteOne(Project => Project.Id == id);
//     }
// }