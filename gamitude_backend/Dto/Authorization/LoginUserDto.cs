using System;
using System.ComponentModel.DataAnnotations;
using gamitude_backend.Model;

namespace gamitude_backend.Dto.Authorization
{
    public class LoginUserDto
    {
        [Required(ErrorMessage="loginRequired")]
        public String login { get; set; }//??can be email or userName or externalId-??

        [Required(ErrorMessage="passwordRequired")]
        public String password { get; set; }
    }
}