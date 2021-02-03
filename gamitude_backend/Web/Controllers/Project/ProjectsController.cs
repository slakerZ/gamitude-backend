using gamitude_backend.Models;
using gamitude_backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using Microsoft.Extensions.Primitives;
using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using gamitude_backend.Dto;
using System.Net;
using Microsoft.Extensions.Logging;
using gamitude_backend.Dto.Project;
using System.Linq;

namespace gamitude_backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ILogger<ProjectsController> _logger;

        //TODO stats verification if Dominant in stats

        private readonly IProjectService _projectService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public ProjectsController(ILogger<ProjectsController> logger,IProjectService projectService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _logger = logger;
            _projectService = projectService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all projects of logged in user.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ControllerResponse<List<GetProjectDto>>>> get()
        {

            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            var projects = await _projectService.getByUserIdAsync(userId);
            return Ok(new ControllerResponse<List<GetProjectDto>>
            {
                data = projects.Select(project => _mapper.Map<GetProjectDto>(project)).ToList()
            });

        }

        /// <summary>
        /// Get project by id.
        /// </summary>
        [HttpGet("{id:length(24)}", Name = "GetProject")]
        public async Task<ActionResult<ControllerResponse<GetProjectDto>>> get(string id)
        {

            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            var project = await _projectService.getByIdAsync(id);
            if(project == null)
            {
                return NotFound();
            }
            if (project.userId != userId)
            {
                throw new UnauthorizedAccessException("Project don't belong to you");
            }
            return Ok( new ControllerResponse<GetProjectDto>
            {
                data = _mapper.Map<GetProjectDto>(project)
            });

        }

        /// <summary>
        /// Create project.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ControllerResponse<GetProjectDto>>> create(CreateProjectDto createProject)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            var project = _mapper.Map<Project>(createProject);
            project.userId = userId;
            project.dateCreated = DateTime.UtcNow;
            await _projectService.createAsync(project);
            return Created(new Uri($"{Request.Path}/{project.id}", UriKind.Relative), new ControllerResponse<GetProjectDto>
            {
                data = _mapper.Map<GetProjectDto>(project)
            });

        }

        /// <summary>
        /// Update project by id.
        /// </summary>
        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult<ControllerResponse<GetProjectDto>>> update(string id, UpdateProjectDto projectIn)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            var project = await _projectService.getByIdAsync(id);

            if (project.userId != userId)
            {
                throw new UnauthorizedAccessException("Project don't belong to you");
            }
            project = _mapper.Map<UpdateProjectDto, Project>(projectIn, project);
            await _projectService.updateAsync(id, project);
            return Ok(new ControllerResponse<GetProjectDto>
            {
                data = _mapper.Map<GetProjectDto>(project)
            });

        }

        /// <summary>
        /// Delete project by id.
        /// </summary>
        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> delete(string id)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            var project = await _projectService.getByIdAsync(id);

            if (project.userId != userId)
            {
                throw new UnauthorizedAccessException("Project don't belong to you");
            }

            await _projectService.deleteByIdAsync(project.id);
            return NoContent();
        }
    }
}
