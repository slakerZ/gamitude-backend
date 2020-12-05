using gamitude_backend.Models;

namespace gamitude_backend.Dto.Rank
{
    public class GetRank
    {
        public string id { get; set; }
        
        public string name { get; set; }

        public GAMITUDE_STYLE Style { get; set; }

        public RANK_TIER Tier { get; set; }

        public RANK_DOMINANT Dominant { get; set; }
                 
        public string imageUrl { get; set; }
        public long priceStrength { get; set; }

        public long priceIntelligence { get; set; }

        public long priceFluency { get; set; }

        public long priceCreativity { get; set; }

        public long priceEuro { get; set; }

    }
}
