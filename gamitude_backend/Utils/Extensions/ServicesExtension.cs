using gamitude_backend.Services;
using Microsoft.Extensions.DependencyInjection;

namespace gamitude_backend.Extensions
{
    public static class ServicesExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IFolderService,FolderService>();
            services.AddScoped<IProjectService,ProjectService>();
            services.AddScoped<IProjectLogService,ProjectLogService>();
            services.AddScoped<IProjectTaskService,ProjectTaskService>();
            services.AddScoped<IJournalService,JournalService>();
            services.AddScoped<IPageService,PageService>();
            services.AddScoped<IRankService,RankService>();
            services.AddScoped<IDailyEnergyService,DailyEnergyService>();
            services.AddScoped<IStatsService,StatsService>();
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<ITimerService,TimerService>();
            services.AddScoped<IUserRankService,UserRankService>();
            services.AddScoped<IAuthorizationService,AuthorizationService>();
        }
    }
}