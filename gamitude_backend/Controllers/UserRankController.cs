
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using gamitude_backend.Dto.Energy;
using gamitude_backend.Dto.Rank;
using gamitude_backend.Dto.Stats;
using gamitude_backend.Dto.UserRank;
using gamitude_backend.BusinessLogic;
using gamitude_backend.Dto;
using gamitude_backend.Models;
using gamitude_backend.Services;

namespace gamitude_backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserRankController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UserRankController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRankService _userRankService;

        public UserRankController(IMapper mapper ,ILogger<UserRankController> logger
        , IHttpContextAccessor httpContextAccessor,IUserRankService userRankService)
        {
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _userRankService = userRankService;
        }


        [HttpGet]
        public async Task<ActionResult<ControllerResponse<GetRank>>> rank()
        {
            try
            {
                _logger.LogInformation("In GET rank");
                string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name).ToString();
                if (null != userId)
                {
                    return new ControllerResponse<GetRank>
                    {
                        data = _mapper.Map<GetRank>(await _userRankService.GetRankByUserId(userId))
                    };
                }
                else
                {
                    _logger.LogError("In GET rank UserId error");

                    return new ControllerResponse<GetRank>
                    {
                        data = null,
                        message = "UserId error",
                        success = false
                    };
                }

            }
            catch (System.Exception e)
            {
                _logger.LogError("Error cached in UserRankController GET rank {error}", e);

                return new ControllerResponse<GetRank>
                {
                    data = null,
                    message = "something went wrong, sorry:(",
                    success = false
                };
            }
        }



        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<ControllerResponse<String>>> Create(CreateUserRank createUserRank)
        {

            try
            {

                if (null != createUserRank.UserId)
                {
                    await _userRankService.CreateAsync(createUserRank.UserId);
                    return new ControllerResponse<String>
                    {
                        data = createUserRank.UserId
                    };
                }
                else
                {
                    return new ControllerResponse<String>
                    {
                        data = null,
                        message = "UserId error",
                        success = false
                    };
                }

            }
            catch (System.Exception)
            {
                return new ControllerResponse<String>
                {
                    data = null,
                    message = "something went wrong, sorry:(",
                    success = false
                };
            }
        }

    }
}
