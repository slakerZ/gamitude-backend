using System;
using System.ComponentModel.DataAnnotations;
using gamitude_backend.Models;

namespace gamitude_backend.Dto.Folder
{

    public class UpdateFolderDto
    {    

        public string name { get; set; }

        public string description { get; set; }

        public string icon { get; set; }

    }
}