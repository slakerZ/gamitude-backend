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

//     public interface IRankService
//     {
//         Task<Rank> GetAsync(String id);
//         Task<String> GetIdByTierDominantAsync(RANK_TIER tier,RANK_DOMINANT dominant,GAMITUDE_STYLE style);
//         //TODO create cms for adding ranks
//         // Rank Create(Rank rank);
//         // Task<Rank> CreateOrAddAsync(Rank rank);
//         // Task UpdateAsync(Rank rank);
//         // void Update(Rank rank);
//         // void Remove(Rank rank);
//         // void Remove(String id);

//     }

//     public class RankService : IRankService
//     {
//         private readonly IMongoCollection<Rank> _Rank;
//         private readonly ILogger<RankService> _logger;

//         public RankService(IDatabaseSettings settings, ILogger<RankService> logger)
//         {
//             var client = new MongoClient(settings.ConnectionString);
//             var database = client.GetDatabase(settings.DatabaseName);

//             _Rank = database.GetCollection<Rank>(settings.RankCollectionName);
//             _logger = logger;
//         }

//         public async Task<Rank> GetAsync(string id)
//         {
//             return await _Rank.Find<Rank>(o => o.Id == id).FirstOrDefaultAsync();
//         }
//         public async Task<String> GetIdByTierDominantAsync(RANK_TIER tier,RANK_DOMINANT dominant,GAMITUDE_STYLE style)
//         {
//             var rank =  await _Rank.Find<Rank>(o => o.Tier == tier && o.Dominant == dominant && o.Style == style).FirstOrDefaultAsync();
//             return rank.Id;
//         }


//         // /// <summary>
//         // /// Gets last weeks energy counting the days for further calculations
//         // /// </summary>
//         // public async Task<GetLastWeekAvgEnergyDto> GetLastWeekAvgEnergyByUserIdAsync(String userId)
//         // {
//         //     GetLastWeekAvgEnergyDto energy = await _Rank.AsQueryable()
//         //          .Where(o => o.UserId == userId && o.Date > DateTime.UtcNow.Date.AddDays(-7))
//         //          .GroupBy(o => o.UserId)
//         //          .Select(o => new GetLastWeekAvgEnergyDto
//         //          {
//         //              Body = o.Sum(o => o.Body),
//         //              Mind = o.Sum(o => o.Mind),
//         //              Emotions = o.Sum(o => o.Emotions),
//         //              Soul = o.Sum(o => o.Soul),
//         //              dayCount = o.Sum(o => 1)

//         //          }).FirstOrDefaultAsync() ?? new GetLastWeekAvgEnergyDto();

//         //     return energy.weekAvg().scaleToPercent();
//         // }

//         // public Rank GetRankByUserId(String userId) =>
//         //     _Rank.Find<Rank>(o => o.UserId == userId).FirstOrDefault();

//         // public Rank Get(String id) =>
//         //     _Rank.Find<Rank>(o => o.Id == id).FirstOrDefault();


//         // public async Task<Rank> CreateOrAddAsync(Rank rank)
//         // {
//         //     try
//         //     {
//         //         var oldDEnergy = await _Rank.Find(o => o.Date == rank.Date && o.UserId == rank.UserId).SingleOrDefaultAsync();
//         //         if (null == oldDEnergy)
//         //         {//Create
//         //             await _Rank.InsertOneAsync(mergeRank(rank, new Rank().init()).validate());
//         //         }
//         //         else
//         //         {//Update   //TODO use mapper for adding
//         //             rank.Id = oldDEnergy.Id;
//         //             rank = mergeRank(rank, oldDEnergy);
//         //             await UpdateAsync(rank.validate());
//         //         }
//         //         return rank;
//         //     }
//         //     catch (Exception e)
//         //     {
//         //         _logger.LogError("Error cached in RankServices CreateOrUpdateAsync {error}", e);
//         //         return null;
//         //     }

//         // }

//         // public Rank Create(Rank Rank)
//         // {
//         //     _Rank.InsertOne(Rank);
//         //     return Rank;
//         // }

//         // public void Update(Rank rank) =>
//         //     _Rank.ReplaceOne(o => o.Id == rank.Id, rank);

//         // public async Task UpdateAsync(Rank rank) =>
//         //     await _Rank.ReplaceOneAsync(o => o.Id == rank.Id, rank);

//         // public void Remove(Rank rank) =>
//         //     _Rank.DeleteOne(o => o.Id == rank.Id);

//         // public void Remove(String id) =>
//         //     _Rank.DeleteOne(o => o.Id == id);


//     }
// }