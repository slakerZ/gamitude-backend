using System;
namespace gamitude_backend.Dto.BulletJournal
{
    public class GetJournalDto
    {
        public string id { get; set; }

        public string userId { get; set; }

        public string projectId { get; set; }

        public string name { get; set; }

        public string icon { get; set; } = "";

        public string description { get; set; }

        public DateTime dateCreated { get; set; }
        
    }
}