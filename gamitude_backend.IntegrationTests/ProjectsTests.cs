using System;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using gamitude_backend.Dto.User;
using gamitude_backend.Dto;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using gamitude_backend.Dto.Authorization;
using System.Net.Http.Headers;
using gamitude_backend.Dto.Project;
using gamitude_backend.Dto.Folder;
using Xunit.Extensions.Ordering;
using System.Linq;
using DeepEqual.Syntax;
using gamitude_backend.Models;
using gamitude_backend.IntegrationTests.Extensions;
using gamitude_backend.Dto.Timer;
using gamitude_backend.Dto.stats;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
[assembly: TestCaseOrderer("Xunit.Extensions.Ordering.TestCaseOrderer", "Xunit.Extensions.Ordering")]
namespace gamitude_backend.IntegrationTests
{
    [CollectionDefinition("Projects environment")]
    public class DatabaseCollection : ICollectionFixture<ProjectsIntegrationTestEnv> { }
    //IClassFixture<T> for context per test 
    // example IClassFixture<WebApplicationFactory<gamitude_backend.Startup>> set database and server per tets
    // ICollectionFixture<T> per collection [Collection("Projects environment")]
    [Collection("Projects environment")]
    public class ProjectsTests : IClassFixture<WebApplicationFactory<gamitude_backend.Startup>>
    {
        private readonly WebApplicationFactory<gamitude_backend.Startup> _factory;
        private readonly String _email = "test@test.pl";
        private readonly String _userName = "test";
        private readonly String _password = "Asdf1234";
        private readonly ProjectsIntegrationTestEnv _projectsEnv;


        public ProjectsTests(WebApplicationFactory<gamitude_backend.Startup> factory, ProjectsIntegrationTestEnv projectsEnv)
        {

            _projectsEnv = projectsEnv;
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, conf) =>
                {
                    Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
                    conf.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        ["DatabaseSettings:ConnectionString"] = _projectsEnv._runner.ConnectionString + "gamitude"
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
            var actualResponse = await client.testSuccessPostAsync<GetUserTokenDto, LoginUserDto>("/api/authorization/login", login);
            // Assert
            Assert.NotNull(actualResponse.data.token);
            Assert.IsType<DateTime>(actualResponse.data.date_expires);
            _projectsEnv.setToken(actualResponse.data.token);
            // Assert.Equal(_email, actualResponse.data.user.email); // user not populated to token response can be checked in token
            // Assert.Equal(_userName, actualResponse.data.user.userName);
        }

        [Fact, Order(20)]
        public async void testGetAllFolders()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _projectsEnv._stringDict.GetValueOrDefault("token"));
            // Act
            var actualResponse = await client.testSuccessGetAsync<List<GetFolderDto>>("/api/folders");
            //Asert
            Assert.Equal(8, actualResponse.data.Count);
        }

        [Fact, Order(30)]
        public async void testGetAllProjects()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _projectsEnv._stringDict.GetValueOrDefault("token"));
            // Act
            var actualResponse = await client.testSuccessGetAsync<List<GetProjectDto>>("/api/projects");
            // Assert
            Assert.Equal(8, actualResponse.data.Count);
        }

        [Fact, Order(40)]
        public async void testAddFolder()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _projectsEnv._stringDict.GetValueOrDefault("token"));
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
                userId = _projectsEnv._parsedToken.Claims.FirstOrDefault(claim => claim.Type == "nameid").Value
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
            _projectsEnv._stringDict.Add("folderId", actualResponseFolder.data.id);

        }

        [Fact, Order(41)]
        public async void testUpdateFolder()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _projectsEnv._stringDict.GetValueOrDefault("token"));
            var folder = new UpdateFolderDto
            {
                description = "test10",
                name = "test11",
                icon = "test13"
            };
            var expectedFolder = new GetFolderDto
            {
                description = folder.description,
                icon = folder.icon,
                name = folder.name,
                userId = _projectsEnv._parsedToken.Claims.FirstOrDefault(claim => claim.Type == "nameid").Value
            };

            // Act
            var actualResponseFolder = await client
                    .testSuccessPutAsync<GetFolderDto, UpdateFolderDto>($"/api/folders/{ _projectsEnv._stringDict.GetValueOrDefault("folderId")}", folder);
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

        }
        [Fact, Order(50)]
        public async void testAddTimer()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _projectsEnv._stringDict.GetValueOrDefault("token"));
            var timer = new CreateTimerDto
            {
                label = "TE",
                name = "test1",
                countDownInfo = new CreateCountDownInfoDto{
                    breakTime = 10,
                    overTime = 5,
                    workTime = 45
                },
                timerType = TIMER_TYPE.TIMER
            };
            var expectedTimer = new GetTimerDto
            {
                label = timer.label,
                timerType = timer.timerType.Value,
                name = timer.name,
                countDownInfo = new GetCountDownInfoDto{
                    breakInterval = timer.countDownInfo.breakInterval,
                    breakTime = timer.countDownInfo.breakTime,
                    longerBreakTime = timer.countDownInfo.longerBreakTime,
                    overTime = timer.countDownInfo.overTime,
                    workTime = timer.countDownInfo.workTime
                },
                userId = _projectsEnv._parsedToken.Claims.FirstOrDefault(claim => claim.Type == "nameid").Value
            };

            // Act
            var actualResponseTimer = await client.testSuccessPostAsync<GetTimerDto, CreateTimerDto>("/api/timers", timer);
            // Assert
            expectedTimer.WithDeepEqual(actualResponseTimer.data)
                    .SkipDefault<DateTime>()
                    .IgnoreSourceProperty(x => x.id)
                    .Assert();

            // Act
            var actualResponseTimerGet = await client.testSuccessGetAsync<GetTimerDto>("/api/timers/" + actualResponseTimer.data.id);
            expectedTimer.id = actualResponseTimer.data.id;
            // Assert
            expectedTimer.WithDeepEqual(actualResponseTimerGet.data)
                    .SkipDefault<DateTime>()
                    .Assert();
            _projectsEnv._stringDict.Add("timerId", actualResponseTimer.data.id);

        }

        [Fact, Order(51)]
        public async void testUpdateTimer()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _projectsEnv._stringDict.GetValueOrDefault("token"));
            var timer = new UpdateTimerDto
            {
                label = "TA",
                name = "test111",
                timerType = TIMER_TYPE.STOPWATCH
            };
            var expectedTimer = new GetTimerDto
            {
                label = timer.label,
                timerType = timer.timerType.Value,
                name = timer.name,
                countDownInfo = new GetCountDownInfoDto{
                    breakInterval = null,
                    breakTime = 0,
                    longerBreakTime = null,
                    overTime = 0,
                    workTime = 0
                },
                userId = _projectsEnv._parsedToken.Claims.FirstOrDefault(claim => claim.Type == "nameid").Value
            };
            
            // Act
            var actualResponseTimer = await client
                    .testSuccessPutAsync<GetTimerDto, UpdateTimerDto>($"/api/timers/{ _projectsEnv._stringDict.GetValueOrDefault("timerId")}", timer);
            // Assert
            expectedTimer.WithDeepEqual(actualResponseTimer.data)
                    .SkipDefault<DateTime>()
                    .IgnoreSourceProperty(x => x.id)
                    .Assert();

            // Act
            var actualResponseTimerGet = await client.testSuccessGetAsync<GetTimerDto>("/api/timers/" + actualResponseTimer.data.id);
            expectedTimer.id = actualResponseTimer.data.id;
            // Assert
            expectedTimer.WithDeepEqual(actualResponseTimerGet.data)
                    .SkipDefault<DateTime>()
                    .Assert();

        }
        [Fact, Order(60)]
        public async void testAddProject()
        {//TODO add all types of projects 
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _projectsEnv._stringDict.GetValueOrDefault("token"));

            var project = new CreateProjectDto
            {
                projectType = PROJECT_TYPE.STAT,
                name = "test1",
                stats = new STATS[] { STATS.BODY },
                dominantStat = STATS.BODY,
                folderId = _projectsEnv._stringDict.GetValueOrDefault("folderId"),
                defaultTimerId = _projectsEnv._stringDict.GetValueOrDefault("timerId")
            };
            var expectedProject = new GetProjectDto
            {
                name = project.name,
                stats = project.stats,
                dominantStat = project.dominantStat,
                projectType = project.projectType,
                folderId = project.folderId,
                defaultTimerId = project.defaultTimerId
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
            _projectsEnv._stringDict.Add("projectId", actualResponseProject.data.id);
        }

        [Fact, Order(61)]
        public async void testUpdateProject()
        {//TODO add all types of projects 
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _projectsEnv._stringDict.GetValueOrDefault("token"));
            var folder = new CreateFolderDto
            {
                description = "test",
                name = "test1",
                icon = "test3"
            };
            var actualResponseFolder = await client.testSuccessPostAsync<GetFolderDto, CreateFolderDto>("/api/folders", folder);
            var responseTimersGet = await client.testSuccessGetAsync<List<GetTimerDto>>("/api/timers");
            var newTimerId = responseTimersGet.data.FirstOrDefault(x => x.id != _projectsEnv._stringDict.GetValueOrDefault("timerId")).id;

            var project = new CreateProjectDto
            {
                projectType = PROJECT_TYPE.ENERGY,
                name = "test10",
                stats = new STATS[] { STATS.EMOTIONS },
                dominantStat = STATS.EMOTIONS,
                folderId = actualResponseFolder.data.id,
                defaultTimerId = newTimerId
            };
            var expectedProject = new GetProjectDto
            {
                name = project.name,
                stats = project.stats,
                dominantStat = project.dominantStat,
                projectType = project.projectType,
                folderId = project.folderId,
                defaultTimerId = project.defaultTimerId
            };

            // Act
            var actualResponseProject = await client
                .testSuccessPutAsync<GetProjectDto, CreateProjectDto>($"/api/projects/{_projectsEnv._stringDict.GetValueOrDefault("projectId")}", project);
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


        [Fact, Order(70)]
        public async void testAddProjectLog()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _projectsEnv._stringDict.GetValueOrDefault("token"));
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
            };
            var oldStats = await client.testSuccessGetAsync<GetStatsDto>("api/statistics/stats");
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