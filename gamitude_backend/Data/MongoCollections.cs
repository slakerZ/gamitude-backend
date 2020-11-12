using gamitude_backend.Models;
using gamitude_backend.Settings;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using Serilog;

namespace gamitude_backend.Data
{
    public interface IDatabaseCollections
    {
        IMongoDatabase database { get; set; }
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
        IMongoCollection<User> users { get; set; }
    }


    public class MongoCollections : IDatabaseCollections
    {
        public IMongoDatabase database { get; set; }
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
        public IMongoCollection<User> users { get; set; }

        public MongoCollections(IDatabaseSettings settings)
        {
            var mongoConnectionUrl = new MongoUrl(settings.connectionString);
            var mongoClientSettings = MongoClientSettings.FromUrl(mongoConnectionUrl);
            // mongoClientSettings.ClusterConfigurator = cb =>
            // {
            //     cb.Subscribe<CommandStartedEvent>(e =>
            //     {
            //         Log.Information($"{e.CommandName} - {e.Command.ToJson()}");
            //     });
            // };

            var client = new MongoClient(mongoClientSettings);
            database = client.GetDatabase(settings.databaseName);

            folders = database.GetCollection<Folder>(settings.foldersCollectionName);
            projects = database.GetCollection<Project>(settings.projectsCollectionName);
            projectLogs = database.GetCollection<ProjectLog>(settings.projectLogsCollectionName);
            projectTasks = database.GetCollection<ProjectTask>(settings.projectTasksCollectionName);
            ranks = database.GetCollection<Rank>(settings.ranksCollectionName);
            themes = database.GetCollection<Theme>(settings.themesCollectionName);
            dailyEnergies = database.GetCollection<DailyEnergy>(settings.dailyEnergiesCollectionName);
            stats = database.GetCollection<Stats>(settings.statsCollectionName);
            timers = database.GetCollection<Timer>(settings.timersCollectionName);
            userTokens = database.GetCollection<UserToken>(settings.usersTokenCollectionName);
            users = database.GetCollection<User>(settings.usersCollectionName);
        }
    }
}