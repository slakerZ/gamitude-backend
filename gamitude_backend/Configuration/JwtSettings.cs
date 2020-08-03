using System;

namespace gamitude_backend.Settings
{
    public class JwtSettings : IJwtSettings
    {
        public String secret { get; set; }
    }
    public interface IJwtSettings
    {
        String secret { get; set; }
    }
}