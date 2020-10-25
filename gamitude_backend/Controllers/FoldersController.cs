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

namespace gamitude_backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class FoldersController : ControllerBase
    {
        private readonly ILogger<FoldersController> _logger;

        //TODO stats verification if Dominant in stats

        private readonly IFolderService _folderService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public FoldersController(ILogger<FoldersController> logger,IFolderService folderService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _logger = logger;
            _folderService = folderService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ControllerResponse<List<Folder>>> get()
        {

            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            return new ControllerResponse<List<Folder>>
            {
                data = await _folderService.getByUserIdAsync(userId)
            };

        }

        [HttpGet("{id:length(24)}", Name = "GetFolder")]
        public async Task<ActionResult<ControllerResponse<Folder>>> get(string id)
        {

            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            var folder = await _folderService.getByIdAsync(id);
            if(folder == null)
            {
                return NotFound();
            }
            if (folder.userId != userId)
            {
                throw new UnauthorizedAccessException("Folder don't belong to you");
            }
            return Ok( new ControllerResponse<Folder>
            {
                data = folder
            });

        }

        [HttpPost]
        public async Task<ActionResult<ControllerResponse<Folder>>> create(Folder folder)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            folder.userId = userId;
            folder.dateCreated = DateTime.UtcNow;
            await _folderService.createAsync(folder);
            return Created(new Uri($"{Request.Path}/{folder.id}", UriKind.Relative), new ControllerResponse<Folder>
            {
                data = folder
            });

        }


        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult<ControllerResponse<Folder>>> update(string id, Folder folderIn)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            var folder = await _folderService.getByIdAsync(id);

            if (folder.userId != userId)
            {
                throw new UnauthorizedAccessException("Folder don't belong to you");
            }
            folder = _mapper.Map<Folder, Folder>(folderIn, folder);
            await _folderService.updateAsync(id, folder);
            return Ok(new ControllerResponse<Folder>
            {
                data = folder
            });

        }


        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> Delete(string id)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            var folder = await _folderService.getByIdAsync(id);

            if (folder.userId != userId)
            {
                throw new UnauthorizedAccessException("Folder don't belong to you");
            }

            await _folderService.deleteByIdAsync(folder.id);
            return NoContent();
        }
    }
}
