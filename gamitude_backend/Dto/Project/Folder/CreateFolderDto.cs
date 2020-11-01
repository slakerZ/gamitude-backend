using System;
using System.ComponentModel.DataAnnotations;
using gamitude_backend.Models;

namespace gamitude_backend.Dto.Folder
{

    public class CreateFolderDto
    {
        [Required]
        public String name { get; set; }

        public String description { get; set; }

        public String icon { get; set; }

    }
}