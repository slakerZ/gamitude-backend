using System;
using System.ComponentModel.DataAnnotations;
using gamitude_backend.Models;

namespace gamitude_backend.Dto.User
{

    public class UpdateUserDto
    {
        public String id { get; set; }

        public String userName { get; set; }

        public String email { get; set; }
    }
}