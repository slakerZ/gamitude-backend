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

        public ProjectsController(IProjectService projectService, IHttpContextAccessor httpContextAccessor)
        {
            _projectService = projectService;
            _httpContextAccessor = httpContextAccessor;
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

                if (project.UserId != userId)
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
                project.UserId = userId;
                project.DateAdded = DateTime.UtcNow;
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
                if (project.UserId != userId)
                {
                    return Unauthorized("Project dont belong to user");
                }
                project = updateProject(project, projectIn);

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
                if (project.UserId != userId)
                {
                    return Unauthorized("Project dont belong to user");
                }

                await _projectService.deleteByIdAsync(project.Id);
                return Ok();

            }
            else
            {
                return NotFound("User token Failure");

            }

        }
        private Project updateProject(Project project, Project projectIn)
        {
            if (null != projectIn.Name)
            {
                project.Name = projectIn.Name;
            }
            if (null != projectIn.PrimaryMethod.ToString())
            {
                project.PrimaryMethod = projectIn.PrimaryMethod;
            }
            if (null != projectIn.ProjectStatus.ToString())
            {
                project.ProjectStatus = projectIn.ProjectStatus;
            }
            if (null != projectIn.Stats)
            {
                project.Stats = projectIn.Stats;
            }
            if (null != projectIn.DominantStat)
            {
                project.DominantStat = projectIn.DominantStat;
            }
            return project;
        }

    }
}
