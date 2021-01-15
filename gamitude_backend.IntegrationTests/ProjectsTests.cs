using System;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using gamitude_backend.Dto.User;
using System.Text.Json;
using System.Net.Http.Json;
using gamitude_backend.Dto;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using gamitude_backend.Dto.Authorization;
using System.Net.Http.Headers;
using gamitude_backend.Dto.Project;
using gamitude_backend.Dto.Folder;
using Xunit.Extensions.Ordering;
using System.Linq;
using System.Security.Claims;
using DeepEqual.Syntax;
using gamitude_backend.Models;
using gamitude_backend.IntegrationTests.Extensions;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
[assembly: TestCaseOrderer("Xunit.Extensions.Ordering.TestCaseOrderer", "Xunit.Extensions.Ordering")]
namespace gamitude_backend.IntegrationTests
{
    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<MongoIntegrationTest> { }
    //IClassFixture<T> for context per test 
    // example IClassFixture<WebApplicationFactory<gamitude_backend.Startup>> set database and server per tets
    // ICollectionFixture<T> per collection [Collection("Database collection")]
    [Collection("Database collection")]
    public class ProjectsTests : IClassFixture<WebApplicationFactory<gamitude_backend.Startup>>
    {
        private readonly WebApplicationFactory<gamitude_backend.Startup> _factory;
        private readonly String _email = "test@test.pl";
        private readonly String _userName = "test";
        private readonly String _password = "Asdf1234";
        private readonly MongoIntegrationTest _mongo;


        public ProjectsTests(WebApplicationFactory<gamitude_backend.Startup> factory, MongoIntegrationTest mongo)
        {

            _mongo = mongo;
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, conf) =>
                {
                    Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
                    conf.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        ["DatabaseSettings:ConnectionString"] = _mongo._runner.ConnectionString + "gamitude"
                    });
                    context.HostingEnvironment.EnvironmentName = "Development";
                });
            });
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/api/version")]
        public async void testVersion(string url)
        {
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);
            var contentString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
        [Fact, Order(0)]
        public async void testCreateUser()
        {
            var client = _factory.CreateClient();

            var user = new CreateUserDto
            {
                email = _email,
                password = _password,
                userName = _userName
            };
            var userResponse = new GetUserDto
            {
                email = user.email,
                userName = user.userName
            };
            var expectedResponse = new ControllerResponse<GetUserDto>
            {
                success = true,
                data = userResponse
            };

            // Act
            var actualResponse = await client.testSuccessPostAsync<GetUserDto, CreateUserDto>("/api/users", user);
            // Assert
            expectedResponse.ShouldDeepEqual(actualResponse);

        }

        [Fact, Order(1)]
        public async void testLogin()
        {
            var client = _factory.CreateClient();

            var login = new LoginUserDto
            {
                login = _userName,
                password = _password,
            };

            // Act
            var actualResponse = await client.testSuccessPostAsync<GetUserTokenDto, LoginUserDto>("/api/authorization/login", login);
            // Assert
            Assert.NotNull(actualResponse.data.token);
            Assert.NotNull(actualResponse.data.date_expires);
            _mongo.setToken(actualResponse.data.token);
            //TODO check username from token
            // Assert.Equal(_email, actualResponse.data.user.email); // user not populated to token response can be checked in token
            // Assert.Equal(_userName, actualResponse.data.user.userName);
        }

        [Fact, Order(2)]
        public async void testGetAllFolders()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _mongo._token);
            // Act
            var actualResponse = await client.testSuccessGetAsync<List<GetFolderDto>>("/api/folders");
            //Asert
            Assert.Equal(8, actualResponse.data.Count);
        }

        [Fact, Order(3)]
        public async void testGetAllProjects()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _mongo._token);
            // Act
            var actualResponse = await client.testSuccessGetAsync<List<GetProjectDto>>("/api/projects");
            // Assert
            Assert.Equal(8, actualResponse.data.Count);
        }

        [Fact, Order(4)]
        public async void testAddFolderAndProject()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _mongo._token);
            var folder = new CreateFolderDto
            {
                description = "test",
                name = "test1",
                icon = "test3"
            };
            var expectedFolder = new GetFolderDto
            {
                description = folder.description,
                icon = folder.icon,
                name = folder.name,
                userId = _mongo._parsedToken.Claims.FirstOrDefault(claim => claim.Type == "nameid").Value
            };

            // Act
            var actualResponseFolder = await client.testSuccessPostAsync<GetFolderDto, CreateFolderDto>("/api/folders", folder);
            // Assert
            expectedFolder.WithDeepEqual(actualResponseFolder.data)
                    .SkipDefault<DateTime>()
                    .IgnoreSourceProperty(x => x.id)
                    .IgnoreDestinationProperty(x => x.dateCreated)
                    .Assert();

            // Act
            var actualResponseFolderGet = await client.testSuccessGetAsync<GetFolderDto>("/api/folders/" + actualResponseFolder.data.id);
            expectedFolder.id = actualResponseFolder.data.id;
            // Assert
            expectedFolder.WithDeepEqual(actualResponseFolderGet.data)
                    .SkipDefault<DateTime>()
                    .IgnoreDestinationProperty(x => x.dateCreated)
                    .Assert();

            var project = new CreateProjectDto
            {
                projectType = PROJECT_TYPE.STAT,
                name = "test1",
                stats = new STATS[] { STATS.BODY },
                dominantStat = STATS.BODY,
                folderId = actualResponseFolderGet.data.id
            };
            var expectedProject = new GetProjectDto
            {
                name = project.name,
                stats = project.stats,
                dominantStat = project.dominantStat,
                projectType = project.projectType,
                folderId = project.folderId
            };

            // Act
            var actualResponseProject = await client.testSuccessPostAsync<GetProjectDto, CreateProjectDto>("/api/projects", project);
            // Assert
            expectedProject.WithDeepEqual(actualResponseProject.data)
                   .SkipDefault<DateTime>()
                   .IgnoreSourceProperty(x => x.id)
                   .IgnoreDestinationProperty(x => x.dateCreated)
                   .Assert();

            // Act
            var actualResponseProjectGet = await client.testSuccessGetAsync<GetProjectDto>("/api/projects/" + actualResponseProject.data.id);
            expectedProject.id = actualResponseProject.data.id;
            // Assert
            expectedProject.WithDeepEqual(actualResponseProjectGet.data)
                    .SkipDefault<DateTime>()
                    .IgnoreDestinationProperty(x => x.dateCreated)
                    .Assert();

        }

        [Fact, Order(5)]
        public async void testAddProjectLog()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _mongo._token);
            var actualResponseProjects = await client.testSuccessGetAsync<List<GetProjectDto>>("/api/projects");

            var project = actualResponseProjects.data.FirstOrDefault(o => o.projectType == PROJECT_TYPE.STAT);
            var projectLog = new CreateProjectLogDto
            {
                log = "test",
                projectId = project.id,
                timeSpend = 90,
                type = PROJECT_TYPE.STAT
            };
            var expectedProjectLog = new GetProjectLogDto
            {
                log = projectLog.log,
                project = project,
                timeSpend = projectLog.timeSpend,
                //TODO add projectType
            };
            // Check Stats and Energy
            // Act
            var actualResponseProjectLog = await client.testSuccessPostAsync<GetProjectLogDto, CreateProjectLogDto>("api/projectlogs", projectLog);
            // Assert
            actualResponseProjectLog.data.WithDeepEqual(expectedProjectLog).IgnoreSourceProperty(x => x.id);

            var projectLogGet = await client.testSuccessGetAsync<GetProjectLogDto>($"api/projectlogs/{actualResponseProjectLog.data.id}");
            expectedProjectLog.id = actualResponseProjectLog.data.id;
            actualResponseProjectLog.data.ShouldDeepEqual(expectedProjectLog);

        }
    }

}