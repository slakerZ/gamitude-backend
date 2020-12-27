using System;
using System.ComponentModel.DataAnnotations;
using gamitude_backend.Models;
namespace gamitude_backend.Dto.Project
{
    public class CreateProjectLogDto
    {
        [Required]
        public string projectId { get; set; }

        public string projectTaskId { get; set; }

        public string log { get; set; }

        [Required]
        public int timeSpend { get; set; }

        [Required]
        public PROJECT_TYPE? type { get; set; }

    }
}