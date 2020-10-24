using gamitude_backend.Services;
using Microsoft.Extensions.DependencyInjection;

namespace gamitude_backend.Extensions
{
    public static class ServicesExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<IAuthorizationService,AuthorizationService>();
            services.AddScoped<IUserRankService,UserRankService>();
            services.AddScoped<IRankService,RankService>();
            services.AddScoped<IStatsService,StatsService>();
            services.AddScoped<IDailyEnergyService,DailyEnergyService>();
            services.AddScoped<IProjectService,ProjectService>();

        }
    }
}