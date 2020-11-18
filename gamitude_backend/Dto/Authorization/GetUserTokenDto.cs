using System;
using gamitude_backend.Models;
using gamitude_backend.Dto.User;
namespace gamitude_backend.Dto.Authorization
{
    public class GetUserTokenDto
    {
        public string userId { get; set; }

        public string token { get; set; }

        public GetUserDto user { get; set; }
        
        public DateTime date_expires { get; set; }
    }
}