using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gamitude_backend.Models
{
    public class UserToken : IBaseEntity
    {
        public int id { get; set; }

        public String userId { get; set; }
    
        [ForeignKey("userId")]
        public User user { get; set; }

        [MaxLength(255)]
        public String token { get; set; }

        public DateTime date_expires { get; set; }

        public DateTime? timeCreated { get ; set ; }
        
        public DateTime? timeUpdated { get ; set ; }
    }


}