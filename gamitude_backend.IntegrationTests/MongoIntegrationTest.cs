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
using System.IdentityModel.Tokens.Jwt;

namespace gamitude_backend.IntegrationTests
{
    public class ProjectsIntegrationTestEnv : IDisposable
    {
        public MongoDbRunner _runner;
        public Dictionary<string, string> _stringDict;
        public JwtSecurityToken _parsedToken;

        public void Dispose()
        {
            _runner.Dispose();
        }
        public void CreateDatabase()
        {
            _runner = MongoDbRunner.Start();
            _runner.Import("gamitude", "ranks", "rankCollection.json", true);
        }
        public void setToken(string token)
        {
            _stringDict.Add("token", token);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            _parsedToken = handler.ReadToken(token) as JwtSecurityToken;
        }
        public ProjectsIntegrationTestEnv()
        {
            CreateDatabase();
            _stringDict = new Dictionary<string, string>();
        }
    }
}
