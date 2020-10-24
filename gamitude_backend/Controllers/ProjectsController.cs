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

namespace gamitude_backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        //TODO stats verification if Dominant in stats

        private readonly IProjectService _projectService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public ProjectsController(IProjectService projectService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _projectService = projectService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Project>>> Get()
        {

            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            if (null != userId)
            {
                return await _projectService.getByUserIdAsync(userId);
            }
            else
            {
                return NotFound("User Failure");
            }

        }

        [HttpGet("{id:length(24)}", Name = "GetProject")]
        public async Task<ActionResult<Project>> Get(string id)
        {

            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            if (null != userId)
            {
                var project = await _projectService.getByIdAsync(id);

                if (project.userId != userId)
                {
                    return Unauthorized("Project dont belong to user");
                }

                return project;

            }
            else
            {
                return NotFound("User Failure");

            }

        }

        [HttpPost]
        public async Task<ActionResult<Project>> Create(Project project)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            if (null != userId)
            {
                project.userId = userId;
                project.dateCreated = DateTime.UtcNow;
                await _projectService.createAsync(project);

                return Created("Create", project);

            }
            else
            {
                return NotFound("User Failure");

            }

        }


        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult<Project>> Update(string id, Project projectIn)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            if (null != userId)
            {
                var project = await _projectService.getByIdAsync(id);
                if (project == null)
                {
                    return NotFound("Project not found");
                }
                if (project.userId != userId)
                {
                    return Unauthorized("Project dont belong to user");
                }
                project = _mapper.Map<Project, Project>(projectIn, project);

                await _projectService.updateAsync(id, project);
                return Ok(project);
            }
            else
            {
                return NotFound("User Token Failure");

            }
        }


        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            if (null != userId)
            {

                var project = await _projectService.getByIdAsync(id);

                if (project == null)
                {
                    return NotFound();
                }
                if (project.userId != userId)
                {
                    return Unauthorized("Project dont belong to user");
                }

                await _projectService.deleteByIdAsync(project.id);
                return Ok();

            }
            else
            {
                return NotFound("User token Failure");

            }

        }
    }
}
