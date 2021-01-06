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
using gamitude_backend.Dto.BulletJournal;
using System.Linq;

namespace gamitude_backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProjectTasksController : ControllerBase
    {
        private readonly ILogger<ProjectTasksController> _logger;

        //TODO stats verification if Dominant in stats

        private readonly IProjectTaskService _projectTaskService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public ProjectTasksController(ILogger<ProjectTasksController> logger, IProjectTaskService projectTaskService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _logger = logger;
            _projectTaskService = projectTaskService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ControllerResponse<List<GetProjectTaskDto>>>> get()
        {

            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            var projectTasks = await _projectTaskService.getByUserIdAsync(userId);
            return Ok(new ControllerResponse<List<GetProjectTaskDto>>
            {
                data = projectTasks.Select(projectTask => _mapper.Map<GetProjectTaskDto>(projectTask)).ToList()
            });

        }

        [HttpGet("journal/{journalId:length(24)}/page/{pageId:length(24)}")]
        public async Task<ActionResult<ControllerResponse<List<GetProjectTaskDto>>>> get(string journalId, string pageId)
        {

            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            var projectTasks = await _projectTaskService.getByJournalIdAndPageIdAsync(userId, journalId, pageId);
            if (projectTasks == null)
            {
                return NotFound();
            }
            return Ok(new ControllerResponse<List<GetProjectTaskDto>>
            {
                data = projectTasks.Select(projectTask => _mapper.Map<GetProjectTaskDto>(projectTask)).ToList()
            });

        }

        [HttpPost]
        public async Task<ActionResult<ControllerResponse<GetProjectTaskDto>>> create(CreateProjectTaskDto createProjectTask)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            var projectTask = _mapper.Map<ProjectTask>(createProjectTask);
            projectTask.userId = userId;
            projectTask.dateCreated = DateTime.UtcNow;
            await _projectTaskService.createAsync(projectTask);
            return Created(new Uri($"{Request.Path}/{projectTask.id}", UriKind.Relative), new ControllerResponse<GetProjectTaskDto>
            {
                data = _mapper.Map<GetProjectTaskDto>(projectTask)
            });

        }


        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult<ControllerResponse<GetProjectTaskDto>>> update(string id, UpdateProjectTaskDto projectTaskIn)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            var projectTask = await _projectTaskService.getByIdAsync(id);

            if (projectTask.userId != userId)
            {
                throw new UnauthorizedAccessException("ProjectTask don't belong to you");
            }
            projectTask = _mapper.Map<UpdateProjectTaskDto, ProjectTask>(projectTaskIn, projectTask);
            await _projectTaskService.updateAsync(id, projectTask);
            return Ok(new ControllerResponse<GetProjectTaskDto>
            {
                data = _mapper.Map<GetProjectTaskDto>(projectTask)
            });

        }


        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> delete(string id)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            var projectTask = await _projectTaskService.getByIdAsync(id);

            if (projectTask.userId != userId)
            {
                throw new UnauthorizedAccessException("ProjectTask don't belong to you");
            }

            await _projectTaskService.deleteByIdAsync(projectTask.id);
            return NoContent();
        }
    }
}
