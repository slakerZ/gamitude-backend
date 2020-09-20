using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gamitude_backend.Dto.User;
using gamitude_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

using Microsoft.EntityFrameworkCore;
using gamitude_backend.Dto;
using gamitude_backend.Dto.Authorization;
using AutoMapper;
using gamitude_backend.Models;

namespace gamitude_backend.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IUserRankService _userRankService;
        private readonly IMapper _mapper;

        public UserController(ILogger<UserController> logger, IUserService userService, IUserRankService userRankService, IMapper mapper)
        {
            _logger = logger;
            _userService = userService;
            _userRankService = userRankService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ControllerResponse<List<GetUserDto>>> all(int offset = 0 ,int limit = 20)
        {
            _logger.LogInformation("In GET all");
            var users = await _userService.getAllAsync(offset,limit);
            return new ControllerResponse<List<GetUserDto>>
            {
                data = users.Select(o => _mapper.Map<GetUserDto>(o)).ToList()
            };
        }

        [HttpGet("{id}")]
        public async Task<ControllerResponse<GetUserDto>> id(String id)
        {
            _logger.LogInformation("In GET id");
            var user = await _userService.getByIdAsync(id);
            return new ControllerResponse<GetUserDto>
            {
                data = _mapper.Map<GetUserDto>(user)
            };
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ControllerResponse<GetUserDto>> create(CreateUserDto newUser)
        {
            _logger.LogInformation("In POST create");
            var user = await _userService.createAsync(_mapper.Map<User>(newUser), newUser.password);
            await _userRankService.CreateAsync(user.Id.ToString());

            return new ControllerResponse<GetUserDto>
            {
                data = _mapper.Map<GetUserDto>(user)
            };
        }

        [HttpPut]
        public async Task<ControllerResponse<GetUserDto>> update(UpdateUserDto updateUser)
        {
            _logger.LogInformation("In PUT update");
            var user = await _userService.updateAsync(_mapper.Map<User>(updateUser));
            return new ControllerResponse<GetUserDto>
            {
                data = _mapper.Map<GetUserDto>(user)
            };
        }

        [HttpPut("password")]
        public async Task<ControllerResponse<GetUserDto>> updatePassword(ChangePasswordUserDto passwordUserDto)
        {
            _logger.LogInformation("In PUT changePassword");
            await _userService.changePasswordAsync(passwordUserDto.id, passwordUserDto.oldPassword,passwordUserDto.newPassword);
            return new ControllerResponse<GetUserDto>();

        }
        [HttpDelete("{id}")]
        public async Task<ControllerResponse<GetUserDto>> delete(String id)
        {
            _logger.LogInformation("In DELETE delete");
            await _userService.deleteByIdAsync(id);
            return new ControllerResponse<GetUserDto>();
        }
    }
}
