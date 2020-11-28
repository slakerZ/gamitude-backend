﻿using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using gamitude_backend.Dto;
using gamitude_backend.Models;
using gamitude_backend.Dto.Project;
using gamitude_backend.Services;
using System;
using System.Collections.Generic;

namespace gamitude_backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProjectLogsController : ControllerBase
    {

        private readonly ILogger<ProjectLogsController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IProjectLogService _projectLogService;

        public ProjectLogsController(ILogger<ProjectLogsController> logger,
                                IHttpContextAccessor httpContextAccessor,
                                IMapper mapper,
                                IProjectLogService projectLogService)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _projectLogService = projectLogService;
        }

        [HttpGet]
        public async Task<ControllerResponse<List<ProjectLog>>> get()
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            return new ControllerResponse<List<ProjectLog>>
            {
                data = await _projectLogService.getByUserIdAsync(userId)
            };

        }

        [HttpGet("{id:length(24)}", Name = "GetProjectLog")]
        public async Task<ActionResult<ControllerResponse<ProjectLog>>> get(string id)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            var projectLog = await _projectLogService.getByIdAsync(id);
            if(projectLog == null)
            {
                return NotFound();
            }
            if (projectLog.userId != userId)
            {
                throw new UnauthorizedAccessException("ProjectLog don't belong to you");
            }
            return Ok( new ControllerResponse<ProjectLog>
            {
                data = projectLog
            });

        }

        [HttpPost]
        public async Task<ActionResult<ControllerResponse<ProjectLog>>> create(CreateProjectLogDto createProjectLog)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            ProjectLog projectLog = _mapper.Map<ProjectLog>(createProjectLog);
            projectLog.userId = userId;
            projectLog  =  await _projectLogService.processCreateProjectLog(projectLog);
            return Created(new Uri($"{Request.Path}/{projectLog.id}", UriKind.Relative), new ControllerResponse<ProjectLog>
            {
                data = projectLog
            });
        }

        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult<ControllerResponse<ProjectLog>>> update(string id, UpdateProjectLogDto projectLogInDto)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            var projectLog = await _projectLogService.getByIdAsync(id);

            if (projectLog.userId != userId)
            {
                throw new UnauthorizedAccessException("ProjectLog don't belong to you");
            }
            var projectLogIn = _mapper.Map<ProjectLog>(projectLogInDto);
            projectLog = _mapper.Map<ProjectLog, ProjectLog>(projectLogIn, projectLog);
            await _projectLogService.updateAsync(id, projectLog);
            return Ok(new ControllerResponse<ProjectLog>
            {
                data = projectLog
            });

        }

        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> Delete(string id)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            await _projectLogService.processDeleteProjectLog(id,userId);

            return NoContent();
        }

    }
}