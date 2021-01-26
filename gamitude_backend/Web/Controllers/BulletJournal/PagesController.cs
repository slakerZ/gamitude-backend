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
    public class PagesController : ControllerBase
    {
        private readonly ILogger<PagesController> _logger;
        private readonly IPageService _pageService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public PagesController(ILogger<PagesController> logger,IPageService pageService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _logger = logger;
            _pageService = pageService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets pages by journal id.
        /// </summary>
        [HttpGet("{journalId:length(24)}")]
        public async Task<ActionResult<ControllerResponse<List<GetPageDto>>>> get(string journalId)
        {

            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            var pages = await _pageService.getByJournalIdAsync(journalId);
            return Ok(new ControllerResponse<List<GetPageDto>>
            {
                data = pages.Select(bullet => _mapper.Map<GetPageDto>(bullet)).ToList()
            });

        }

        /// <summary>
        /// Create page.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ControllerResponse<GetPageDto>>> create(CreatePageDto createPage)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            var bullet = _mapper.Map<Page>(createPage);
            bullet.userId = userId;
            bullet.dateCreated = DateTime.UtcNow;
            await _pageService.createAsync(bullet);
            return Created(new Uri($"{Request.Path}/{bullet.id}", UriKind.Relative), new ControllerResponse<GetPageDto>
            {
                data = _mapper.Map<GetPageDto>(bullet)
            });

        }

        /// <summary>
        /// Update page by id.
        /// </summary>
        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult<ControllerResponse<GetPageDto>>> update(string id, UpdatePageDto bulletIn)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            var bullet = await _pageService.getByIdAsync(id);

            if (bullet.userId != userId)
            {
                throw new UnauthorizedAccessException("Page don't belong to you");
            }
            bullet = _mapper.Map<UpdatePageDto, Page>(bulletIn, bullet);
            await _pageService.updateAsync(id, bullet);
            return Ok(new ControllerResponse<GetPageDto>
            {
                data = _mapper.Map<GetPageDto>(bullet)
            });

        }

        /// <summary>
        /// Delete page by id.
        /// </summary>
        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> delete(string id)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            var bullet = await _pageService.getByIdAsync(id);

            if (bullet.userId != userId)
            {
                throw new UnauthorizedAccessException("Page don't belong to you");
            }

            await _pageService.deleteByIdAsync(bullet.id);
            return NoContent();
        }
    }
}
