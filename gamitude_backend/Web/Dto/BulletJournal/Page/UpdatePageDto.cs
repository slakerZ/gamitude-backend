using System;
using System.Collections.Generic;
namespace gamitude_backend.Dto.BulletJournal
{
    public class UpdatePageDto
    {
        public string journalId { get; set; }

        public string name { get; set; }

        public UpdateBeetwenDaysDto beetwenDays { get; set; }
        public string icon { get; set; }

        public DateTime dateCreated { get; set; }
    }
    
    public class UpdateBeetwenDaysDto
    {
        public int? fromDay { get; set; }

        public int? toDay { get; set; }
    }
}