using System.ComponentModel.DataAnnotations;

namespace gamitude_backend.Dto.User
{

    public class CreateUserDto
    {
        [Required]
        public string userName { get; set; }

        [Required]
        public string email { get; set; }

        [Required]
        public string password { get; set; }

    }
}