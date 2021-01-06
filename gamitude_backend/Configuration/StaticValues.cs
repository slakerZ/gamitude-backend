using System;
using Microsoft.Extensions.Hosting;

namespace gamitude_backend.Configuration
{
    public static class StaticValues
    {
        public static int workDayLength { get; set; } = 8 * 60;
        // public static int powerMultiplier { get; set; } = 3;
        public static Boolean IsDevelopment()
        {
            return (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development);

        }
    }

}