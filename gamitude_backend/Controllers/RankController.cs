
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
using gamitude_backend.Dto.stats;
using gamitude_backend.Dto;
using gamitude_backend.Models;
using gamitude_backend.Services;
using System.Linq;
using System.Collections.Generic;

namespace gamitude_backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RankController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RankController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRankService _rankService;

        public RankController(IMapper mapper, ILogger<RankController> logger
        , IHttpContextAccessor httpContextAccessor, IRankService rankService)
        {
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _rankService = rankService;
        }


        [HttpGet]
        public async Task<ActionResult<ControllerResponse<List<GetRank>>>> get(int page = 1,int limit = 20,String sortBy="name")
        {
            _logger.LogInformation("In GET rank");
            var ranks = await _rankService.getAllAsync(page,limit,sortBy);
            return new ControllerResponse<List<GetRank>>
            {
                data = ranks.Select(o => _mapper.Map<GetRank>(o)).ToList()
                
            };
        }
        [HttpGet("user")]
        public async Task<ActionResult<ControllerResponse<List<GetRank>>>> getUser()
        {
            _logger.LogInformation("In GET user rank");
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            var ranks = await _rankService.getAllByUserIdAsync(userId);
            return new ControllerResponse<List<GetRank>>
            {
                data = ranks.Select(o => _mapper.Map<GetRank>(o)).ToList()
            };
        }
        [HttpGet("current")]
        public async Task<ActionResult<ControllerResponse<GetRank>>> getUserCurrent()
        {
            _logger.LogInformation("In GET user rank");
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            var rank = await _rankService.getCurrentByUserIdAsync(userId);
            return new ControllerResponse<GetRank>
            {
                data = _mapper.Map<GetRank>(rank)
            };
        }
    }
}
