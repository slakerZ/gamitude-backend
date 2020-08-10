using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace gamitude_backend.Models
{
    //Using interface over class because of one class inhertiance in c# user class can only derive from IdentityUser class
    public interface IBaseEntity
    {
        // [Column("time_created")]
        DateTime? timeCreated { get; set; }
        // [Column("time_updated")]
        DateTime? timeUpdated { get; set; }
    }

}