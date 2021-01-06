using System.ComponentModel.DataAnnotations;
using gamitude_backend.Models;

namespace gamitude_backend.Dto.Rank
{
    public class PostSetRankDto
    {
        [Required]
        public string id { get; set; }
        
    }
}
