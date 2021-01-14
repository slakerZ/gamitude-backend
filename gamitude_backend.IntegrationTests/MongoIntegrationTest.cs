using System;
using Xunit;
using Mongo2Go;
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using gamitude_backend.Settings;
using Microsoft.AspNetCore.TestHost;
using gamitude_backend.Extensions;
using System.Text.Json;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;

namespace gamitude_backend.IntegrationTests
{
    public class MongoIntegrationTest
    {
        public MongoDbRunner _runner;

        public void CreateDatabase()
        {
            _runner = MongoDbRunner.Start();
            _runner.Import("gamitude", "ranks", "rankCollection.json", true);
        }
    }
}
