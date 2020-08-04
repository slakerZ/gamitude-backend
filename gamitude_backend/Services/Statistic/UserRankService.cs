// using gamitude_backend.Models;
 
// using System.Linq;
// using System;
// using System.Threading.Tasks;
// using Microsoft.Extensions.Logging;
// using gamitude_backend.Dto.Energy;
 
// using gamitude_backend.Settings;
// using System.ComponentModel.DataAnnotations;

// namespace gamitude_backend.Services
// {

//     public interface IUserRankService
//     {
//         Task<Rank> GetRankByUserId(String userId);
//         Task UpdateAsync(UserRank userRank);
//         Task CreateAsync(String userId);
//     }

//     public class UserRankService : IUserRankService
//     {
//         private readonly IMongoCollection<UserRank> _UserRank;
//         private readonly ILogger<UserRankService> _logger;
//         private readonly IRankService _ranksService;


//         public UserRankService(IDatabaseSettings settings, ILogger<UserRankService> logger, IRankService ranksService)
//         {
//             var client = new MongoClient(settings.ConnectionString);
//             var database = client.GetDatabase(settings.DatabaseName);

//             _UserRank = database.GetCollection<UserRank>(settings.UserRankCollectionName);
//             _logger = logger;
//             _ranksService = ranksService;
//         }
//         public async Task CreateAsync(String userId) =>
//             await _UserRank.InsertOneAsync(new UserRank
//             {
//                 UserId = userId,
//                 RankId = await _ranksService.GetIdByTierDominantAsync(RANK_TIER.F, RANK_DOMINANT.BALANCED, GAMITUDE_STYLE.DEFAULT)
//             });

//         public async Task UpdateAsync(UserRank userRank) {
//             var olduserRank =  await GetUserRankByUserId(userRank.UserId);
//             userRank.Id = olduserRank.Id;
//             await _UserRank.ReplaceOneAsync(o => o.Id == userRank.Id, userRank,new ReplaceOptions{IsUpsert=true});
//         }

//         private Task<UserRank> GetUserRankByUserId(String userId)
//         {
//             return _UserRank.Find<UserRank>(o => o.UserId == userId).FirstOrDefaultAsync();
            
//         }

//         public async Task<Rank> GetRankByUserId(String userId)
//         {
//             var userRank = _UserRank.Find<UserRank>(o => o.UserId == userId).FirstOrDefault();
//             return await _ranksService.GetAsync(userRank.RankId);
//         }

//     }
// }