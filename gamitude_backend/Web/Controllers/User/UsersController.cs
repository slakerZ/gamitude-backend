using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gamitude_backend.Dto.User;
using gamitude_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using gamitude_backend.Dto;
using AutoMapper;
using gamitude_backend.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace gamitude_backend.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UsersController(ILogger<UsersController> logger, IUserService userService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _logger = logger;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ControllerResponse<GetUserDto>>> id(string id)
        {
            _logger.LogInformation("In GET id");
            var user = await _userService.getByIdAsync(id);
            return Ok(new ControllerResponse<GetUserDto>
            {
                data = _mapper.Map<GetUserDto>(user)
            });
        }
        [HttpGet("money")]
        public async Task<ActionResult<ControllerResponse<long>>> money()
        {
            _logger.LogInformation("In GET money");
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            var money = await _userService.getMoneyAsync(userId);
            return Ok(new ControllerResponse<long>
            {
                data = money
            });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<ControllerResponse<GetUserDto>>> create(CreateUserDto newUser)
        {
            _logger.LogInformation("In POST create");
            var user = await _userService.createAsync(_mapper.Map<User>(newUser), newUser.password);
            _logger.LogInformation("after create");

            return Created(new Uri($"{Request.Path}/{user.Id}", UriKind.Relative), new ControllerResponse<GetUserDto>
            {
                data = _mapper.Map<GetUserDto>(user)
            });
        }

        [AllowAnonymous]
        [HttpPost("verifyEmail/{login}/{token}")]
        public async Task<ActionResult<ControllerResponse<string>>> verifyEmail(string login, string token)
        {
            _logger.LogInformation("In POST verifyEmail");
            await _userService.verifyEmail(login, token);
            _logger.LogInformation("after create");

            return Ok(new ControllerResponse<string> { data = "Email verified" });
        }

        [HttpPut]
        public async Task<ActionResult<ControllerResponse<GetUserDto>>> update(string id, UpdateUserDto updateUser)
        {
            _logger.LogInformation("In PUT update");
            string tokenUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            var user = await _userService.updateAsync(tokenUserId, _mapper.Map<User>(updateUser));
            return Ok(new ControllerResponse<GetUserDto>
            {
                data = _mapper.Map<GetUserDto>(user)
            });
        }

        [HttpPut("password")]
        public async Task<ActionResult<ControllerResponse<GetUserDto>>> updatePassword(string id, ChangePasswordUserDto passwordUserDto)
        {
            _logger.LogInformation("In PUT changePassword");
            string tokenUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            await _userService.changePasswordAsync(tokenUserId, passwordUserDto.oldPassword, passwordUserDto.newPassword);
            return Ok(new ControllerResponse<GetUserDto>());
        }

        [HttpDelete]
        public async Task<ActionResult> delete(string id)
        {
            _logger.LogInformation("In DELETE delete");
            string tokenUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            await _userService.deleteByIdAsync(tokenUserId);
            return NoContent();
        }
    }
}
