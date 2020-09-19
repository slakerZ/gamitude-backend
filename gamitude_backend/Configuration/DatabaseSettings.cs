using System;

namespace gamitude_backend.Settings
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public String projectsCollectionName { get; set; }
        public String usersTokenCollectionName { get; set; }
        public String dailyEnergyCollectionName { get; set; }
        public String dailyStatsCollectionName { get; set; }
        public String rankCollectionName { get; set; }
        public String timeSpendCollectionName { get; set; }
        public String userRankCollectionName { get; set; }
        public String connectionString { get; set; }
        public String databaseName { get; set; }

    }
    public interface IDatabaseSettings
    {
        String projectsCollectionName { get; set; }
        String usersTokenCollectionName { get; set; }
        String dailyEnergyCollectionName { get; set; }
        String dailyStatsCollectionName { get; set; }
        String rankCollectionName { get; set; }
        String timeSpendCollectionName { get; set; }
        String userRankCollectionName { get; set; }
        String connectionString { get; set; }
        String databaseName { get; set; }
    }
}