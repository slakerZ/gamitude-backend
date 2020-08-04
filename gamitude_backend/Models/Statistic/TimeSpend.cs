// using System;
// using System.Collections.Generic;
// using System.Linq;
 
 

// namespace gamitude_backend.Models
// {
//     public class TimeSpend
//     {

//         public string Id { get; set; }

//         public string UserId { get; set; }

//         public string ProjectId { get; set; }

//         public PROJECT_TYPE? ProjectType { get; set; }

//         public int Duration { get; set; }

//         public STATS? DominantStat { get; set; }

//         public STATS[] Stats { get; set; }

//         /// <summary>
//         /// Calculates the wages. Values depends on how many stats u wanna boost.
//         /// </summary>
//         public Dictionary<STATS, int> getWages()
//         {
//             Dictionary<STATS, int> wageMap = new Dictionary<STATS, int>();
//             int dominantStat = 0;
//             int stat = 0;
//             switch (Stats.GetLength(0))
//             {
//                 case 1:
//                     stat = 0;
//                     dominantStat = 1;
//                     break;
//                 case 2:
//                     stat = 3;
//                     dominantStat = 4;
//                     break;
//                 case 3:
//                     stat = 2;
//                     dominantStat = 3;
//                     break;
//                 case 4:
//                     stat = 2;
//                     dominantStat = 4;
//                     break;

//             }
//             foreach (var s in Stats)
//             {
//                 if (s == DominantStat)
//                 {
//                     wageMap.Add(s, dominantStat);
//                 }
//                 else
//                 {
//                     wageMap.Add(s, stat);
//                 }
//             }
//             return wageMap;
//         }

//     }


// }