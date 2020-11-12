using System;
using System.ComponentModel.DataAnnotations;
using gamitude_backend.Models;

namespace gamitude_backend.Dto.Folder
{

    public class CreateFolderDto
    {
        [Required]
        public string name { get; set; }

        public string description { get; set; }

        [Required]
        public string icon { get; set; }

    }
}