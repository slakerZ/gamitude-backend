using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace gamitude_backend.Models
{
    public class User : IdentityUser , IBaseEntity
    {
        [PersonalData]
        [MaxLength(255)]
        public string name { get; set; }

        public DateTime? timeCreated { get ; set ; }
        
        public DateTime? timeUpdated { get ; set ; }

        public List<Project> offers { get; set; } = new List<Project>();

    }
}