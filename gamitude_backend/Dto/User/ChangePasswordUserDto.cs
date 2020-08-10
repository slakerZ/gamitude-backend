using System;
using System.ComponentModel.DataAnnotations;
using gamitude_backend.Models;

namespace gamitude_backend.Dto.User
{

    public class ChangePasswordUserDto
    {
        [Required(ErrorMessage="idRequired")]
        public String id { get; set; }
        
        [Required(ErrorMessage="passwordRequired")]
        public String oldPassword { get; set; }
        [Required(ErrorMessage="passwordRequired")]
        public String newPassword { get; set; }

    }
}