using System;
using System.ComponentModel.DataAnnotations;

namespace gamitude_backend.Dto.BulletJournal
{
    public class CreateJournalDto
    {
        public string projectId { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string icon { get; set; }

        public string description { get; set; }
        
    }
}