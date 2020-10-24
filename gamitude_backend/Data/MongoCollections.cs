using gamitude_backend.Models;
using gamitude_backend.Settings;
using MongoDB.Driver;

namespace gamitude_backend.Data
{
    public interface IDatabaseCollections
    {
        IMongoCollection<Folder> folders { get; set; }
        IMongoCollection<Project> projects { get; set; }
        IMongoCollection<ProjectLog> projectLogs { get; set; }
        IMongoCollection<ProjectTask> projectTasks { get; set; }
        IMongoCollection<Rank> ranks { get; set; }
        IMongoCollection<Theme> themes { get; set; }
        IMongoCollection<DailyEnergy> dailyEnergies { get; set; }
        IMongoCollection<Stats> stats { get; set; }
        IMongoCollection<Timer> timers { get; set; }
        IMongoCollection<UserToken> userTokens { get; set; }
        IMongoCollection<UserRank> userRanks { get; set; }
    }


    public class MongoCollections : IDatabaseCollections
    {
        private readonly IMongoDatabase _database;
        public IMongoCollection<Folder> folders { get; set; }
        public IMongoCollection<Project> projects { get; set; }
        public IMongoCollection<ProjectLog> projectLogs { get; set; }
        public IMongoCollection<ProjectTask> projectTasks { get; set; }
        public IMongoCollection<Rank> ranks { get; set; }
        public IMongoCollection<Theme> themes { get; set; }
        public IMongoCollection<DailyEnergy> dailyEnergies { get; set; }
        public IMongoCollection<Stats> stats { get; set; }
        public IMongoCollection<Timer> timers { get; set; }
        public IMongoCollection<UserToken> userTokens { get; set; }
        public IMongoCollection<UserRank> userRanks { get; set; }

        public MongoCollections(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.connectionString);
            var database = client.GetDatabase(settings.databaseName);

            folders = database.GetCollection<Folder>(settings.foldersCollectionName);
            projects = database.GetCollection<Project>(settings.projectsCollectionName);
            projectLogs = database.GetCollection<ProjectLog>(settings.projectLogsCollectionName);
            projectTasks = database.GetCollection<ProjectTask>(settings.projectTasksCollectionName);
            ranks = database.GetCollection<Rank>(settings.ranksCollectionName);
            themes = database.GetCollection<Theme>(settings.themesCollectionName);
            dailyEnergies = database.GetCollection<DailyEnergy>(settings.dailyEnergyCollectionName);
            stats = database.GetCollection<Stats>(settings.statsCollectionName);
            timers = database.GetCollection<Timer>(settings.timersCollectionName);
            userTokens = database.GetCollection<UserToken>(settings.usersTokenCollectionName);
            userRanks = database.GetCollection<UserRank>(settings.userRankCollectionName);
        }
    }
}