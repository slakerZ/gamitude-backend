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
using gamitude_backend.Dto.Timer;
using System.Linq;

namespace gamitude_backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TimersController : ControllerBase
    {
        private readonly ILogger<TimersController> _logger;

        //TODO stats verification if Dominant in stats

        private readonly ITimerService _timerService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public TimersController(ILogger<TimersController> logger,ITimerService timerService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _logger = logger;
            _timerService = timerService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all timers of logged in user.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ControllerResponse<List<GetTimerDto>>>> get()
        {

            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            var timers = await _timerService.getByUserIdAsync(userId);
            return Ok( new ControllerResponse<List<GetTimerDto>>
            {
                data = timers.Select(timer => _mapper.Map<GetTimerDto>(timer)).ToList() 
            });

        }

        /// <summary>
        /// Get timer by id.
        /// </summary>
        [HttpGet("{id:length(24)}", Name = "GetTimer")]
        public async Task<ActionResult<ControllerResponse<GetTimerDto>>> get(string id)
        {

            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            var timer = await _timerService.getByIdAsync(id);
            if(timer == null)
            {
                return NotFound();
            }
            if (timer.userId != userId)
            {
                throw new UnauthorizedAccessException("Timer don't belong to you");
            }
            return Ok( new ControllerResponse<GetTimerDto>
            {
                data = _mapper.Map<GetTimerDto>(timer)
            });

        }

        /// <summary>
        /// Create timer.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ControllerResponse<GetTimerDto>>> create(CreateTimerDto newTimer)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            var timer = _mapper.Map<Timer>(newTimer);
            timer.userId = userId;
            timer.dateCreated = DateTime.UtcNow;
            await _timerService.createAsync(timer);
            return Created(new Uri($"{Request.Path}/{timer.id}", UriKind.Relative), new ControllerResponse<GetTimerDto>
            {
                data = _mapper.Map<GetTimerDto>(timer)
            });

        }

        /// <summary>
        /// Update timer.
        /// </summary>
        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult<ControllerResponse<GetTimerDto>>> update(string id, UpdateTimerDto timerIn)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            var timer = await _timerService.getByIdAsync(id);

            if (timer.userId != userId)
            {
                throw new UnauthorizedAccessException("Timer don't belong to you");
            }
            timer = _mapper.Map<UpdateTimerDto, Timer>(timerIn, timer);
            await _timerService.updateAsync(id, timer);
            return Ok(new ControllerResponse<GetTimerDto>
            {
                data = _mapper.Map<GetTimerDto>(timer)
            });

        }

        /// <summary>
        /// Delete timer.
        /// </summary>
        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> Delete(string id)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            var timer = await _timerService.getByIdAsync(id);

            if (timer.userId != userId)
            {
                throw new UnauthorizedAccessException("Timer don't belong to you");
            }

            await _timerService.deleteByIdAsync(timer.id);
            return NoContent();
        }
    }
}
