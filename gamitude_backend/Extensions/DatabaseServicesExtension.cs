using gamitude_backend.Services;
using Microsoft.Extensions.DependencyInjection;

namespace gamitude_backend.Extensions
{
    public static class DatabaseServicesExtension
    {
        public static void AddCustomDatabaseServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<IAuthorizationService,AuthorizationService>();

        }
    }
}