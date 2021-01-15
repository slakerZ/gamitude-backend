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
            var response = await client.PostAsJsonAsync("/api/users", user);
            var contentString = await response.Content.ReadAsStringAsync();
            var actualResponse = System.Text.Json.JsonSerializer.Deserialize<ControllerResponse<GetUserDto>>(contentString);
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            expectedResponse.ShouldDeepEqual(actualResponse);

        }

        [Fact, Order(10)]
        public async void testLogin()
        {
            var client = _factory.CreateClient();

            var login = new LoginUserDto
            {
                login = _userName,
                password = _password,
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/authorization/login", login);
            var contentString = await response.Content.ReadAsStringAsync();
            var actualResponse = System.Text.Json.JsonSerializer.Deserialize<ControllerResponse<GetUserTokenDto>>(contentString);
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.True(actualResponse.success);
            Assert.NotNull(actualResponse.data.token);
            Assert.NotNull(actualResponse.data.date_expires);
            _mongo.setToken(actualResponse.data.token);
            // Assert.Equal(_email, actualResponse.data.user.email); // user not populated to token response can be checked in token
            // Assert.Equal(_userName, actualResponse.data.user.userName);
        }

        [Fact, Order(20)]
        public async void testGetAllFolders()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _mongo._token);
            // Act
            var response = await client.GetAsync("/api/folders");
            var contentString = await response.Content.ReadAsStringAsync();
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var actualResponse = System.Text.Json.JsonSerializer.Deserialize<ControllerResponse<List<GetFolderDto>>>(contentString);
            Assert.True(actualResponse.success);
            Assert.Equal(8, actualResponse.data.Count);
        }

        [Fact, Order(30)]
        public async void testGetAllProjects()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _mongo._token);
            // Act
            var response = await client.GetAsync("/api/projects");
            var contentString = await response.Content.ReadAsStringAsync();
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var actualResponse = JsonConvert.DeserializeObject<ControllerResponse<List<GetProjectDto>>>(contentString);
            Assert.True(actualResponse.success);
            Assert.Equal(8, actualResponse.data.Count);
        }

        [Fact, Order(30)]
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
            var expectedResponse = new ControllerResponse<GetFolderDto>
            {
                data = expectedFolder,
                success = true
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/folders", folder);
            var contentString = await response.Content.ReadAsStringAsync();
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var actualResponse = JsonConvert.DeserializeObject<ControllerResponse<GetFolderDto>>(contentString);
            Assert.True(actualResponse.success);

            // Act
            var responseGet = await client.GetAsync("/api/folders/" + actualResponse.data.id);
            var contentStringGet = await responseGet.Content.ReadAsStringAsync();
            var actualResponseGet = JsonConvert.DeserializeObject<ControllerResponse<GetFolderDto>>(contentStringGet);
            // Assert
            Assert.True(actualResponse.success);
            actualResponse.WithDeepEqual(actualResponseGet)
                    .SkipDefault<DateTime>()
                    .IgnoreDestinationProperty(x => x.data.dateCreated)
                    .Assert();

            var project = new CreateProjectDto
            {
                projectType = PROJECT_TYPE.STAT,
                name = "test1",
                stats = new STATS[] { STATS.BODY },
                dominantStat = STATS.BODY,
                folderId = actualResponseGet.data.id
            };
            var expectedProject = new GetProjectDto
            {
                name = project.name,
                stats = project.stats,
                dominantStat = project.dominantStat,
                projectType = project.projectType,
                folderId = project.folderId
            };
            var expectedResponseProject = new ControllerResponse<GetProjectDto>
            {
                data = expectedProject,
                success = true
            };

            // Act
            var responseProject = await client.PostAsJsonAsync("/api/projects", project);
            var contentStringProject = await responseProject.Content.ReadAsStringAsync();
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var actualResponseProject = JsonConvert.DeserializeObject<ControllerResponse<GetProjectDto>>(contentStringProject);
            Assert.True(actualResponseProject.success);

            // Act
            var responseProjectGet = await client.GetAsync("/api/projects/" + actualResponseProject.data.id);
            var contentStringProjectGet = await responseProjectGet.Content.ReadAsStringAsync();
            var actualResponseProjectGet = JsonConvert.DeserializeObject<ControllerResponse<GetProjectDto>>(contentStringProjectGet);
            // Assert
            Assert.True(actualResponseProjectGet.success);
            actualResponseProject.WithDeepEqual(actualResponseProjectGet)
                    .SkipDefault<DateTime>()
                    .IgnoreDestinationProperty(x => x.data.dateCreated)
                    .Assert();

        }
    }

}