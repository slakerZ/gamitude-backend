namespace gamitude_backend.Dto.User
{
    public class VerifyEmailNewDto
    {
        public string login { get; set; }
        public string email { get; set; }
        public string token { get; set; }
    }
}