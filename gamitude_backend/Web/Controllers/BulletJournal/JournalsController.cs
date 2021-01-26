using gamitude_backend.Models;
using gamitude_backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
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
    public class JournalsController : ControllerBase
    {
        private readonly ILogger<JournalsController> _logger;
        private readonly IJournalService _journalService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public JournalsController(ILogger<JournalsController> logger, IJournalService journalService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _logger = logger;
            _journalService = journalService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all journals of logged in user.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ControllerResponse<List<GetJournalDto>>>> get()
        {

            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            var journals = await _journalService.getByUserIdAsync(userId);
            return Ok(new ControllerResponse<List<GetJournalDto>>
            {
                data = journals.Select(bullet => _mapper.Map<GetJournalDto>(bullet)).ToList()
            });

        }
        /// <summary>
        /// Gets journal by id.
        /// </summary>
        [HttpGet("{id:length(24)}", Name = "GetJournal")]
        public async Task<ActionResult<ControllerResponse<GetJournalDto>>> get(string id)
        {

            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            var bullet = await _journalService.getByIdAsync(id);
            if (bullet == null)
            {
                return NotFound();
            }
            if (bullet.userId != userId)
            {
                throw new UnauthorizedAccessException("Journal don't belong to you");
            }
            return Ok(new ControllerResponse<GetJournalDto>
            {
                data = _mapper.Map<GetJournalDto>(bullet)
            });

        }

        /// <summary>
        /// Create journal.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ControllerResponse<GetJournalDto>>> create(CreateJournalDto createJournal)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            var bullet = _mapper.Map<Journal>(createJournal);
            bullet.userId = userId;
            bullet.dateCreated = DateTime.UtcNow;
            await _journalService.createAsync(bullet);
            return Created(new Uri($"{Request.Path}/{bullet.id}", UriKind.Relative), new ControllerResponse<GetJournalDto>
            {
                data = _mapper.Map<GetJournalDto>(bullet)
            });

        }

        /// <summary>
        /// Update journal by id.
        /// </summary>
        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult<ControllerResponse<GetJournalDto>>> update(string id, UpdateJournalDto bulletIn)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            var bullet = await _journalService.getByIdAsync(id);

            if (bullet.userId != userId)
            {
                throw new UnauthorizedAccessException("Journal don't belong to you");
            }
            bullet = _mapper.Map<UpdateJournalDto, Journal>(bulletIn, bullet);
            await _journalService.updateAsync(id, bullet);
            return Ok(new ControllerResponse<GetJournalDto>
            {
                data = _mapper.Map<GetJournalDto>(bullet)
            });

        }


        /// <summary>
        /// Delete journal by id.
        /// </summary>
        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> delete(string id)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            var bullet = await _journalService.getByIdAsync(id);

            if (bullet.userId != userId)
            {
                throw new UnauthorizedAccessException("Journal don't belong to you");
            }

            await _journalService.deleteByIdAsync(bullet.id);
            return NoContent();
        }
    }
}
