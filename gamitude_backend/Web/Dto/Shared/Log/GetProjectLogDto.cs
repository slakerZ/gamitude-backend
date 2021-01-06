using System;
using gamitude_backend.Dto.BulletJournal;
using gamitude_backend.Models;
namespace gamitude_backend.Dto.Project
{
    public class GetProjectLogDto
    {
        public string id { get; set; }

        public string log { get; set; }

        public int timeSpend { get; set; }

        public GetProjectDto project { get; set; }
        public GetProjectTaskDto projectTask { get; set; }


    }
}