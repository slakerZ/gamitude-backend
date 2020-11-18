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

namespace gamitude_backend.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(ILogger<UsersController> logger, IUserService userService, IMapper mapper)
        {
            _logger = logger;
            _userService = userService;
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

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<ControllerResponse<GetUserDto>>> create(CreateUserDto newUser)
        {
            _logger.LogInformation("In POST create");
            var user = await _userService.createAsync(_mapper.Map<User>(newUser), newUser.password);
            _logger.LogInformation("after create");

            return Created(new Uri($"{Request.Path}/{user.Id}", UriKind.Relative),new ControllerResponse<GetUserDto>
            {
                data = _mapper.Map<GetUserDto>(user)
            });
        }

        [HttpPut]
        public async Task<ActionResult<ControllerResponse<GetUserDto>>> update(UpdateUserDto updateUser)
        {
            _logger.LogInformation("In PUT update");
            var user = await _userService.updateAsync(_mapper.Map<User>(updateUser));
            return Ok( new ControllerResponse<GetUserDto>
            {
                data = _mapper.Map<GetUserDto>(user)
            });
        }

        [HttpPut("password")]
        public async Task<ActionResult<ControllerResponse<GetUserDto>>> updatePassword(ChangePasswordUserDto passwordUserDto)
        {
            _logger.LogInformation("In PUT changePassword");
            await _userService.changePasswordAsync(passwordUserDto.id, passwordUserDto.oldPassword,passwordUserDto.newPassword);
            return Ok(new ControllerResponse<GetUserDto>());

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> delete(string id)
        {
            _logger.LogInformation("In DELETE delete");
            await _userService.deleteByIdAsync(id);
            return NoContent();
        }
    }
}
