using System.ComponentModel.DataAnnotations;

namespace gamitude_backend.Dto.User
{

    public class ChangeEmailUserDto
    {
        
        [Required(ErrorMessage="emailRequired")]
        public string newEmail { get; set; }

    }
}