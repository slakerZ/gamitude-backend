using System;

namespace gamitude_backend.Settings
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string foldersCollectionName { get; set; }
        public string projectsCollectionName { get; set; }
        public string projectLogsCollectionName { get; set; }
        public string projectTasksCollectionName { get; set; }
        public string journalsCollectionName { get; set; }
        public string pagesCollectionName { get; set; }
        public string ranksCollectionName { get; set; }
        public string themesCollectionName { get; set; }
        public string dailyEnergiesCollectionName { get; set; }
        public string statsCollectionName { get; set; }
        public string timersCollectionName { get; set; }
        public string usersTokenCollectionName { get; set; }
        public string usersRanksCollectionName { get; set; }
        public string usersRankCollectionName { get; set; }
        public string usersThemesCollectionName { get; set; }
        public string usersThemeCollectionName { get; set; }
        public string usersCollectionName { get; set; }
        public string connectionString { get; set; }
        public string databaseName { get; set; }

    }
    public interface IDatabaseSettings
    {
        string foldersCollectionName { get; set; }
        string projectsCollectionName { get; set; }
        string projectLogsCollectionName { get; set; }
        string projectTasksCollectionName { get; set; }
        string journalsCollectionName { get; set; }
        string pagesCollectionName { get; set; }
        string ranksCollectionName { get; set; }
        string themesCollectionName { get; set; }
        string dailyEnergiesCollectionName { get; set; }
        string statsCollectionName { get; set; }
        string timersCollectionName { get; set; }
        string usersTokenCollectionName { get; set; }
        string usersRanksCollectionName { get; set; }
        string usersRankCollectionName { get; set; }
        string usersThemesCollectionName { get; set; }
        string usersThemeCollectionName { get; set; }
        string usersCollectionName { get; set; }
        string connectionString { get; set; }
        string databaseName { get; set; }
    }
}