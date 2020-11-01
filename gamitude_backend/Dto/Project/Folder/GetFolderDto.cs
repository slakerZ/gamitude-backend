using System;
using gamitude_backend.Models;

namespace gamitude_backend.Dto.Folder
{

    public class GetFolderDto
    {
        public String id { get; set; }

        public String userId { get; set; }

        public String name { get; set; }

        public String description { get; set; }

        public String icon { get; set; }

        public DateTime dateCreated { get; set; }
    }
}