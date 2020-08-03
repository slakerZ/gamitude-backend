using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using gamitude_backend.BusinessLogic;
using gamitude_backend.Dto;
using gamitude_backend.Models;
namespace gamitude_backend.Controllers
{
    [Route("api/stats/[controller]")]
    [Authorize]
    [ApiController]
    public class TimeController : ControllerBase
    {

        private readonly ILogger<TimeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly ITimeManager _timeManager;

        public TimeController(ILogger<TimeController> logger,
                                IHttpContextAccessor httpContextAccessor,
                                IMapper mapper,
                                ITimeManager timeManager)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _timeManager = timeManager;
        }
        [HttpPost]
        public async Task<ActionResult<ControllerResponse<GetTimeSpend>>> Create(CreateTimeSpend createTimeSpend)
        {

            try
            {

                string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name).ToString();
                if (null != userId)
                {
                    TimeSpend timeSpend = _mapper.Map<TimeSpend>(createTimeSpend);
                    timeSpend.UserId = userId;
                    return new ControllerResponse<GetTimeSpend>
                    {
                        data = await _timeManager.manageTime(timeSpend)
                    };
                }
                else
                {
                    return new ControllerResponse<GetTimeSpend>
                    {
                        data = null,
                        message = "UserId error",
                        success = false
                    };
                }

            }
            catch (System.Exception)
            {
                return new ControllerResponse<GetTimeSpend>
                {
                    data = null,
                    message = "something went wrong, sorry:(",
                    success = false
                };
            }
        }

    }
}
