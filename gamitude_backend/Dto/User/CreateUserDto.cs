using System;
using System.ComponentModel.DataAnnotations;
using gamitude_backend.Models;

namespace gamitude_backend.Dto.User
{

    public class CreateUserDto
    {
        [Required]
        public String userName { get; set; }

        [Required]
        public String email { get; set; }

        [Required]
        public String password { get; set; }

    }
}