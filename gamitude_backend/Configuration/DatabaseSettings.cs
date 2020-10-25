using System;

namespace gamitude_backend.Settings
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public String foldersCollectionName { get; set; }
        public String projectsCollectionName { get; set; }
        public String projectLogsCollectionName { get; set; }
        public String projectTasksCollectionName { get; set; }
        public String ranksCollectionName { get; set; }
        public String themesCollectionName { get; set; }
        public String dailyEnergiesCollectionName { get; set; }
        public String statsCollectionName { get; set; }
        public String timersCollectionName { get; set; }
        public String usersTokenCollectionName { get; set; }
        public String usersRanksCollectionName { get; set; }
        public String usersRankCollectionName { get; set; }
        public String usersThemesCollectionName { get; set; }
        public String usersThemeCollectionName { get; set; }
        public String connectionString { get; set; }
        public String databaseName { get; set; }

    }
    public interface IDatabaseSettings
    {
        String foldersCollectionName { get; set; }
        String projectsCollectionName { get; set; }
        String projectLogsCollectionName { get; set; }
        String projectTasksCollectionName { get; set; }
        String ranksCollectionName { get; set; }
        String themesCollectionName { get; set; }
        String dailyEnergiesCollectionName { get; set; }
        String statsCollectionName { get; set; }
        String timersCollectionName { get; set; }
        String usersTokenCollectionName { get; set; }
        String usersRanksCollectionName { get; set; }
        String usersRankCollectionName { get; set; }
        String usersThemesCollectionName { get; set; }
        String usersThemeCollectionName { get; set; }
        String connectionString { get; set; }
        String databaseName { get; set; }
    }
}