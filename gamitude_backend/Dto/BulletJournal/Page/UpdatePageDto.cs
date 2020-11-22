using System;
using System.Collections.Generic;
namespace gamitude_backend.Dto.BulletJournal
{
    public class UpdatePageDto
    {
        public string journalId { get; set; }

        public string name { get; set; }

        public int? fromDay { get; set; }

        public int? toDay { get; set; }

        public string icon { get; set; }

        public DateTime dateCreated { get; set; }
    }
}