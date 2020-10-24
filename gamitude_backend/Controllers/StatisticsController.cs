
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using gamitude_backend.Dto.Energy;
using gamitude_backend.Dto.stats;
using gamitude_backend.Dto;
using gamitude_backend.Services;
using AutoMapper;

namespace gamitude_backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class StatisticsController : ControllerBase
    {

        private readonly ILogger<StatisticsController> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDailyEnergyService _dailyEnergyService;
        private readonly IStatsService _dailyStatsService;

        public StatisticsController(ILogger<StatisticsController> logger,
         IMapper mapper, 
         IHttpContextAccessor httpContextAccessor, 
         IDailyEnergyService dailyEnergyService,
          IStatsService dailyStatsService)
        {
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _dailyEnergyService = dailyEnergyService;
            _dailyStatsService = dailyStatsService;
        }


        [HttpGet]
        public async Task<ActionResult<ControllerResponse<GetStatsDto>>> stats()
        {
            try
            {
                _logger.LogInformation("In GET GetStats");
                string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name).ToString();
                if (null != userId)
                {
                    return new ControllerResponse<GetStatsDto>
                    {
                        data = _mapper.Map<GetStatsDto>(await _dailyStatsService.getByUserIdAsync(userId))
                    };
                }
                else
                {
                    _logger.LogError("In GET GetStats userId error");

                    return new ControllerResponse<GetStatsDto>
                    {
                        data = null,
                        message = "userId error",
                        success = false
                    };
                }

            }
            catch (System.Exception e)
            {
                _logger.LogError("Error cached in StatisticsController GET GetStats {error}", e);

                return new ControllerResponse<GetStatsDto>
                {
                    data = null,
                    message = "something went wrong, sorry:(",
                    success = false
                };
            }
        }

        [HttpGet]
        public async Task<ActionResult<ControllerResponse<GetLastWeekAvgEnergyDto>>> energy()
        {
            try
            {
                _logger.LogInformation("In GET GetEnergy");

                string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name).ToString();
                if (null != userId)
                {
                    return new ControllerResponse<GetLastWeekAvgEnergyDto>
                    {
                        data = await _dailyEnergyService.GetLastWeekAvgEnergyByUserIdAsync(userId)
                    };
                }
                else
                {
                    _logger.LogError("In GET GetEnergy userId error");

                    return new ControllerResponse<GetLastWeekAvgEnergyDto>
                    {
                        data = null,
                        message = "userId error",
                        success = false
                    };
                }

            }
            catch (System.Exception e)
            {
                _logger.LogError("Error cached in StatisticsController GET GetEnergy {error}", e);

                return new ControllerResponse<GetLastWeekAvgEnergyDto>
                {
                    data = null,
                    message = "something went wrong, sorry:(",
                    success = false
                };
            }
        }
    }
}
