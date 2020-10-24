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
        public String dailyEnergyCollectionName { get; set; }
        public String statsCollectionName { get; set; }
        public String timersCollectionName { get; set; }
        public String usersTokenCollectionName { get; set; }
        public String userRankCollectionName { get; set; }
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
        String dailyEnergyCollectionName { get; set; }
        String statsCollectionName { get; set; }
        String timersCollectionName { get; set; }
        String usersTokenCollectionName { get; set; }
        String userRankCollectionName { get; set; }
        String connectionString { get; set; }
        String databaseName { get; set; }
    }
}