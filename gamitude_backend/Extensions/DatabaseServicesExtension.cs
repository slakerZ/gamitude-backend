using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using gamitude_backend.Data;
using gamitude_backend.Settings;

namespace gamitude_backend.Extensions
{
    public static class DatabaseServicesExtension
    {
        public static void ConfigureDatabaseConnection(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<DataContext>(c =>
                 //  services.AddDbContextPool<DataContext>(c => //for better performance
                 c.UseSqlServer(configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>().connectionString));

        }
    }
}