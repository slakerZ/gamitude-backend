using gamitude_backend.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace gamitude_backend.Extensions
{
    public static class RepositoriesExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IFolderRepository,FolderRepository>();
            services.AddScoped<IProjectRepository,ProjectRepository>();
            services.AddScoped<IProjectLogRepository,ProjectLogRepository>();
            services.AddScoped<IProjectTaskRepository,ProjectTaskRepository>();
            services.AddScoped<IJournalRepository,JournalRepository>();
            services.AddScoped<IPageRepository,PageRepository>();
            services.AddScoped<IRankRepository,RankRepository>();
            services.AddScoped<IThemeRepository,ThemeRepository>();
            services.AddScoped<IDailyEnergyRepository,DailyEnergyRepository>();
            services.AddScoped<IStatsRepository,StatsRepository>();
            services.AddScoped<IUserRankRepository,UserRankRepository>();
            services.AddScoped<IUserRanksRepository,UserRanksRepository>();
            services.AddScoped<IUserThemeRepository,UserThemeRepository>();
            services.AddScoped<IUserThemesRepository,UserThemesRepository>();
            services.AddScoped<ITimerRepository,TimerRepository>();
            services.AddScoped<IMoneyRepository,MoneyRepository>();
        }
    }
}