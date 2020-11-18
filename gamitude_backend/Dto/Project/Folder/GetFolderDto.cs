using System;
using gamitude_backend.Models;

namespace gamitude_backend.Dto.Folder
{

    public class GetFolderDto
    {
        public string id { get; set; }

        public string userId { get; set; }

        public string name { get; set; }

        public string description { get; set; }

        public string icon { get; set; } = "";

        public DateTime dateCreated { get; set; }
    }
}