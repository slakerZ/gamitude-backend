using gamitude_backend.Models;

using MongoDB.Driver;
using gamitude_backend.Repositories;
using gamitude_backend.Data;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using gamitude_backend.Exceptions;

namespace gamitude_backend.Services
{


    public interface IRankService : IRankRepository
    {
        Task<Rank> getCurrentByUserIdAsync(string id);
        Task<Rank> purchaseRankIdAsync(string userId, string rankId, CURRENCY currency);
        Task<Rank> selectRankIdAsync(string userId, string rankId);
        Task<IReadOnlyList<Rank>> getAllByUserIdAsync(string id);
    }

    public class RankService : RankRepository, IRankService
    {
        private readonly IMongoCollection<Rank> _ranks;
        private readonly IUserRankRepository _userRankRepository;
        private readonly IUserRanksRepository _userRanksRepository;
        private readonly IMoneyRepository _moneyRepository;
        private readonly IStatsRepository _statsRepository;

        public RankService(IDatabaseCollections dbCollections,
          IUserRankRepository userRankRepository,
          IUserRanksRepository userRanksRepository,
          IMoneyRepository moneyRepository,
          IStatsRepository statsRepository) : base(dbCollections)
        {
            _ranks = dbCollections.ranks;
            _userRankRepository = userRankRepository;
            _userRanksRepository = userRanksRepository;
            _moneyRepository = moneyRepository;
            _statsRepository = statsRepository;
        }
        public async Task<Rank> getCurrentByUserIdAsync(string id)
        {
            var userRankId = await _userRankRepository.getByUserIdAsync(id);
            var rank = await getByIdAsync(userRankId);
            return rank;
        }
        public async Task<IReadOnlyList<Rank>> getAllByUserIdAsync(string id)
        {
            var userRankIds = await _userRanksRepository.getByUserIdAsync(id);
            return await getByIdAsync(userRankIds);
        }

        public async Task<Rank> purchaseRankIdAsync(string userId, string rankId, CURRENCY currency)
        {
            var rank = await getByIdAsync(rankId);
            if (currency == CURRENCY.REAL)
            {
                var money = await _moneyRepository.getMoneyByUserIdAsync(userId);
                var afterPurchaseMoney = money - rank.priceEuro;
                if (afterPurchaseMoney >= 0)
                {
                    await _userRanksRepository.addAsync(userId, rank.id);
                    await _moneyRepository.createOrUpdateMoneyAsync(userId, afterPurchaseMoney);
                    return rank;
                }
            }
            else
            {
                var stats = await _statsRepository.getByUserIdAsync(userId);
                stats.creativity -= rank.priceCreativity;
                stats.fluency -= rank.priceFluency;
                stats.intelligence -= rank.priceIntelligence;
                stats.strength -= rank.priceStrength;
                if (stats.creativity >= 0 
                    && stats.fluency >= 0 
                    && stats.intelligence >= 0 
                    && stats.strength >= 0)
                {
                    await _userRanksRepository.addAsync(userId,rankId);
                    await _statsRepository.updateAsync(stats.id,stats);
                }
            }

            throw new ShopException("There is no enough resources on your account");

        }

        public async Task<Rank> selectRankIdAsync(string userId, string rankId)
        {
            var userRanks = await _userRanksRepository.getByUserIdAsync(userId);
            if (userRanks.Contains(rankId))
            {
                await _userRankRepository.createOrUpdateAsync(userId, rankId);
                return await getByIdAsync(userId);
            }
            throw new ShopException("There is no purchased rank with coresponding id");
        }
    }
}