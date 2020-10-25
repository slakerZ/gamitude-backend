
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

            _logger.LogInformation("In GET GetStats");
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();


            return Ok(new ControllerResponse<GetStatsDto>
            {
                data = _mapper.Map<GetStatsDto>(await _dailyStatsService.getByUserIdAsync(userId))
            });

        }

        [HttpGet]
        public async Task<ActionResult<ControllerResponse<GetLastWeekAvgEnergyDto>>> energy()
        {

            _logger.LogInformation("In GET GetEnergy");

            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name).ToString();

            return Ok(new ControllerResponse<GetLastWeekAvgEnergyDto>
            {
                data = await _dailyEnergyService.GetLastWeekAvgEnergyByUserIdAsync(userId)
            });



        }
    }
}
