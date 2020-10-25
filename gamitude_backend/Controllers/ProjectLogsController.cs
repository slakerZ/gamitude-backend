using System.Security.Claims;
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

    }
}
