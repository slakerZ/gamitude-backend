using System;

namespace gamitude_backend.Settings
{
    public class JwtSettings : IJwtSettings
    {
        public string secret { get; set; }
    }
    public interface IJwtSettings
    {
        string secret { get; set; }
    }
}