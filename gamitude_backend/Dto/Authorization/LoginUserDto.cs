using System;
using System.ComponentModel.DataAnnotations;
using gamitude_backend.Models;

namespace gamitude_backend.Dto.Authorization
{
    public class LoginUserDto
    {
        [Required(ErrorMessage="loginRequired")]
        public string login { get; set; }//??can be email or userName or externalId-??

        [Required(ErrorMessage="passwordRequired")]
        public string password { get; set; }
    }
}