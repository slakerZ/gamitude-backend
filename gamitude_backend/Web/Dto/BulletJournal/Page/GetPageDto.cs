using System;
using System.Collections.Generic;
namespace gamitude_backend.Dto.BulletJournal
{
    public class GetPageDto
    {
        public string id { get; set; }

        public string userId { get; set; }

        public string journalId { get; set; }

        public string name { get; set; }

        public int fromDay { get; set; }

        public int toDay { get; set; }

        public string icon { get; set; } = "";

        public DateTime dateCreated { get; set; }
    }
}