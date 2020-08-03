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

namespace gamitude_backend.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        //TODO stats verification if Dominant in stats

        private readonly ProjectService _projectService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProjectsController(ProjectService projectService, IHttpContextAccessor httpContextAccessor)
        {
            _projectService = projectService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public ActionResult<List<Project>> Get()
        {

            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name).ToString();

            if (null != userId)
            {
                return _projectService.GetProjectsByUserId(userId);
            }
            else
            {
                return NotFound("User Failure");
            }

        }

        [HttpGet("{id:length(24)}", Name = "GetProject")]
        public ActionResult<Project> Get(string id)
        {

            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name).ToString();

            if (null != userId)
            {
                return _projectService.Get(id);

            }
            else
            {
                return NotFound("User Failure");

            }

        }

        [HttpPost]
        public ActionResult<Project> Create(Project project)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name).ToString();

            if (null != userId)
            {
                project.UserId = userId;
                project.DateAdded = DateTime.UtcNow;
                Project newProject = _projectService.Create(project);

                return Created("Create", newProject);

            }
            else
            {
                return NotFound("User Failure");

            }

        }


        [HttpPut("{id:length(24)}")]
        public ActionResult<Project> Update(string id, Project projectIn)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name).ToString();

            if (null != userId)
            {
                var project = _projectService.Get(id);
                if (project == null)
                {
                    return NotFound("Project not found");
                }
                project = updateProject(project, projectIn);

                _projectService.Update(id, project);
                return Ok(project);
            }
            else
            {
                return NotFound("User Failure");

            }
        }


        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name).ToString();

            if (null != userId)
            {

                var project = _projectService.Get(id);

                if (project == null)
                {
                    return NotFound();
                }

                _projectService.Remove(project.Id);
                return Ok();

            }
            else
            {
                return NotFound("User Failure");

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
