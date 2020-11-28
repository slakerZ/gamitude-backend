using System;
using System.Collections.Generic;
namespace gamitude_backend.Dto.BulletJournal
{
    public class GetProjectTaskDto
    {
        public string id { get; set; }

        public string userId { get; set; }

        public string journalId { get; set; }

        public string projectId { get; set; }

        public string name { get; set; }

        public string description { get; set; }

        public string note { get; set; }

        public List<string> tags { get; set; }

        public DateTime? deadLine { get; set; }

        public DateTime dateCreated { get; set; }

        public DateTime? dateFinished { get; set; }

    }
}