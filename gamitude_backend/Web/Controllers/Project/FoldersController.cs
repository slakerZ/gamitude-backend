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
using gamitude_backend.Dto.Folder;
using System.Linq;

namespace gamitude_backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class FoldersController : ControllerBase
    {
        private readonly ILogger<FoldersController> _logger;
        private readonly IFolderService _folderService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public FoldersController(ILogger<FoldersController> logger, IFolderService folderService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _logger = logger;
            _folderService = folderService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ControllerResponse<List<GetFolderDto>>>> get()
        {

            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            var folders = await _folderService.getByUserIdAsync(userId);
            return Ok(new ControllerResponse<List<GetFolderDto>>
            {
                data = folders.Select(folder => _mapper.Map<GetFolderDto>(folder)).ToList()
            });

        }

        [HttpGet("{id:length(24)}", Name = "GetFolder")]
        public async Task<ActionResult<ControllerResponse<GetFolderDto>>> get(string id)
        {

            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            var folder = await _folderService.getByIdAsync(id);
            if (folder == null)
            {
                return NotFound();
            }
            if (folder.userId != userId)
            {
                throw new UnauthorizedAccessException("Folder don't belong to you");
            }
            return Ok(new ControllerResponse<GetFolderDto>
            {
                data = _mapper.Map<GetFolderDto>(folder)
            });

        }

        [HttpPost]
        public async Task<ActionResult<ControllerResponse<GetFolderDto>>> create(CreateFolderDto createFolder)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            var folder = _mapper.Map<Folder>(createFolder);
            folder.userId = userId;
            folder.dateCreated = DateTime.UtcNow;
            await _folderService.createAsync(folder);
            return Created(new Uri($"{Request.Path}/{folder.id}", UriKind.Relative), new ControllerResponse<GetFolderDto>
            {
                data = _mapper.Map<GetFolderDto>(folder)
            });

        }


        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult<ControllerResponse<GetFolderDto>>> update(string id, UpdateFolderDto folderIn)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            var folder = await _folderService.getByIdAsync(id);

            if (folder.userId != userId)
            {
                throw new UnauthorizedAccessException("Folder don't belong to you");
            }
            folder = _mapper.Map<UpdateFolderDto, Folder>(folderIn, folder);
            await _folderService.updateAsync(id, folder);
            return Ok(new ControllerResponse<GetFolderDto>
            {
                data = _mapper.Map<GetFolderDto>(folder)
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
