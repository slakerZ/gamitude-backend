using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using gamitude_backend.Dto.Rank;
using gamitude_backend.Dto.stats;
using gamitude_backend.Dto;
using gamitude_backend.Models;
using gamitude_backend.Services;

namespace gamitude_backend.BusinessLogic
{//TODO talk about rank zones ???
    public interface IRankManager
    {
        Task manageRank(String userId);
    }

    public class RankManager : IRankManager
    {
        /// <summary>
        /// This class is responisible for rank selection after user session 
        /// </summary>

        private readonly ILogger<RankManager> _logger;
        private readonly IMapper _mapper;
        private readonly IRankService _ranksService;
        private readonly IUserRankService _userRankService;
        private readonly IDailyStatsService _dailyStatsService;
        private String userId;
        private GetLastWeekAvgStatsDto stats;
        private UserRank userRank;

        public RankManager(ILogger<RankManager> logger, IMapper mapper, IRankService ranksService, IUserRankService userRankService
                            , IDailyStatsService dailyStatsService)
        {
            _logger = logger;
            _mapper = mapper;
            _ranksService = ranksService;
            _userRankService = userRankService;
            _dailyStatsService = dailyStatsService;
        }
        public async Task manageRank(String userId)
        {
            try
            {
                this.userId = userId;
                stats = await _dailyStatsService.GetLastWeekAvgStatsByUserIdAsync(userId);
                await calculateRank();
                await _userRankService.UpdateAsync(userRank);
            }
            catch (Exception e)
            {
                _logger.LogError("Error cached in RankManager manageRank {error}", e);
                throw e;
            }
        }
        private async Task calculateRank()
        {
            RANK_TIER tier = RANK_TIER.F;
            RANK_DOMINANT dominant = RANK_DOMINANT.BALANCED;
            GAMITUDE_STYLE style = GAMITUDE_STYLE.DEFAULT;
            var statsList = new List<int> { stats.strength, stats.intelligence, stats.fluency, stats.creativity };
            var sum = statsList.Sum();
            var max = statsList.Max();
            if(statsList.All(o => o==statsList.First()))
            {
                dominant = RANK_DOMINANT.BALANCED;
            }
            else if (max == stats.strength)
            {
                dominant = RANK_DOMINANT.STRENGHT;
            }
            else if (max == stats.intelligence)
            {
                dominant = RANK_DOMINANT.INTELLIGENCE;
            }
            else if (max == stats.fluency)
            {
                dominant = RANK_DOMINANT.FLUENCY;
            }
            else if (max == stats.creativity)
            {
                dominant = RANK_DOMINANT.CREATIVITY;
            }

            if (sum < 40)
            {
                tier = RANK_TIER.F;
            }
            else if (sum >= 40 && sum < 90)
            {
                tier = RANK_TIER.D;
            }
            else if (sum >= 90 && sum < 150)
            {
                tier = RANK_TIER.C;
            }
            else if (sum >= 150 && sum < 230)
            {
                tier = RANK_TIER.B;
            }
            else if (sum >= 230 && sum < 320)
            {
                tier = RANK_TIER.A;
            }
            else if (sum >= 320)
            {
                tier = RANK_TIER.S;
            }

            userRank =  new UserRank
            {
                userId = userId,
                RankId = await _ranksService.GetIdByTierDominantAsync(tier, dominant, style)
            };

        }

    }
}