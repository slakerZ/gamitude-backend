using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using gamitude_backend.Settings;
using gamitude_backend.Configuration;

namespace gamitude_backend.Extensions
{
    public static class SettingsExtension
    {
        public static void ReadSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IDatabaseSettings>(configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>());
            services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
            services.Configure<EmailSenderSettings>(configuration.GetSection(nameof(EmailSenderSettings)));
        }
    }
}