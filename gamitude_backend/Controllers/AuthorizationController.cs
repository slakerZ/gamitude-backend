
using gamitude_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using gamitude_backend.Dto.Authorization;
using System;
using System.Threading.Tasks;
using gamitude_backend.Dto;
using AutoMapper;
using gamitude_backend.Dto.User;

namespace gamitude_backend.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly ILogger<AuthorizationController> _logger;
        private readonly Services.IAuthorizationService _authoriaztionService;
        private readonly IMapper _mapper;

        public AuthorizationController(ILogger<AuthorizationController> logger, IAuthorizationService authoriaztionService, IMapper mapper)

        {
            _logger = logger;
            _authoriaztionService = authoriaztionService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ControllerResponse<GetUserTokenDto>>> login(LoginUserDto user)
        {
            _logger.LogInformation("In POST login");
            var userToken = await _authoriaztionService.authorizeUserAsync(user.login, user.password);
            return Ok( new ControllerResponse<GetUserTokenDto>
            {
                data = _mapper.Map<GetUserTokenDto>(userToken)
            });
        }
    }
}
