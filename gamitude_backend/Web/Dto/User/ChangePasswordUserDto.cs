using System.ComponentModel.DataAnnotations;

namespace gamitude_backend.Dto.User
{

    public class ChangePasswordUserDto
    {
        
        [Required(ErrorMessage="passwordRequired")]
        public string oldPassword { get; set; }
        [Required(ErrorMessage="passwordRequired")]
        public string newPassword { get; set; }

    }
}