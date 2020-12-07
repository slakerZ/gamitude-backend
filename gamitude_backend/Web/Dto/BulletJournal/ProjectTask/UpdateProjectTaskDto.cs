using System;
using System.Collections.Generic;
namespace gamitude_backend.Dto.BulletJournal
{
    public class UpdateProjectTaskDto
    {
        public string journalId { get; set; }

        public string projectId { get; set; }

        public string name { get; set; }

        public string description { get; set; }

        public string note { get; set; }

        public List<string> tags { get; set; }

        public DateTime? deadLine { get; set; }

    }
}