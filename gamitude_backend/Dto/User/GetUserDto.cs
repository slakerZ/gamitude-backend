using System;
using gamitude_backend.Models;

namespace gamitude_backend.Dto.User
{

    public class GetUserDto
    {
        public String id { get; set; }
        public String userName { get; set; }

        public String email { get; set; }

    }
}