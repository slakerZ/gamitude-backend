using System;
namespace gamitude_backend.Dto.BulletJournal
{
    public class UpdateJournalDto
    {
        public string projectId { get; set; }

        public string name { get; set; }

        public string icon { get; set; }

        public string description { get; set; }
        
    }
}