using System;

namespace gamitude_backend.Settings
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public String connectionString { get; set; }
    }
    public interface IDatabaseSettings
    {
        String connectionString { get; set; }
    }
}