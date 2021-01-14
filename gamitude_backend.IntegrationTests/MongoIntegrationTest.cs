using System;
using Xunit;
using Mongo2Go;
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace gamitude_backend.IntegrationTests
{
    public class MongoIntegrationTest
    {
        internal static MongoDbRunner _runner;

        internal static void CreateDatabase()
        {
            _runner = MongoDbRunner.Start();
            _runner.Import("gamitude", "ranks", @"../db/rankCollection.json", true);
        }
    }

    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT","Development");
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            builder.UseConfiguration(configuration);
        }
    }
}
