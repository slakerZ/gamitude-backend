using System.ComponentModel.DataAnnotations;

namespace gamitude_backend.Dto.User
{
    public class VerifyEmailDto
    {
        [Required]
        public string login { get; set; }
        [Required]
        public string token { get; set; }
    }
}