using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace gamitude_backend.Models
{
    //Using interface over class because of one class inhertiance in c# user class can only derive from IdentityUser class
    public interface IBaseEntity
    {
        DateTime? timeCreated { get; set; }
        DateTime? timeUpdated { get; set; }
    }

}