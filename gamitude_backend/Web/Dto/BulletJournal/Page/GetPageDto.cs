using System;
using System.Collections.Generic;
using gamitude_backend.Models;

namespace gamitude_backend.Dto.BulletJournal
{
    public class GetPageDto
    {
        public string id { get; set; }

        public string userId { get; set; }

        public string journalId { get; set; }

        public string name { get; set; }

        public GetBeetwenDaysDto beetwenDays { get; set; }

        public string icon { get; set; } = "";

        public DateTime dateCreated { get; set; }

        public PAGE_TYPE pageType { get; set; }
    }
    public class GetBeetwenDaysDto
    {
        public int fromDay { get; set; }

        public int toDay { get; set; }
    }
}