using System;
using System.Collections.Generic;
using gamitude_backend.Models;

namespace gamitude_backend.Dto.BulletJournal
{
    public class UpdatePageDto
    {
        public string journalId { get; set; }

        public string name { get; set; }

        public UpdateBeetwenDaysDto beetwenDays { get; set; }
        public string icon { get; set; }

        public DateTime dateCreated { get; set; }

        public PAGE_TYPE? pageType { get; set; }
    }
    
    public class UpdateBeetwenDaysDto
    {
        public int? fromDay { get; set; }

        public int? toDay { get; set; }
    }
}