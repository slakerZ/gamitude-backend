using System.ComponentModel.DataAnnotations;
using gamitude_backend.Models;

namespace gamitude_backend.Dto.Rank
{
    public class PostPurchaseRankDto
    {
        [Required]
        public string id { get; set; }
        
        [Required]
        public CURRENCY currency { get; set; }
    }
}
