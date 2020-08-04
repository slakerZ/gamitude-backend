using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using gamitude_backend.Settings;

namespace gamitude_backend.Extensions
{
    public static class SettingsExtension
    {
        public static void ReadSettings(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<DatabaseSettings>(configuration.GetSection(nameof(DatabaseSettings)));
            services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));

        }
    }
}