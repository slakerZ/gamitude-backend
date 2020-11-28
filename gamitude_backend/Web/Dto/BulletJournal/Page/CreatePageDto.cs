using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace gamitude_backend.Dto.BulletJournal
{
    public class CreatePageDto
    {
        [Required]
        public string journalId { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public int fromDay { get; set; }

        [Required]
        public int toDay { get; set; }

        [Required]
        public string icon { get; set;}

    }
}