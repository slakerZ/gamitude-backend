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
using ExpectedObjects;
using gamitude_backend.Dto.Authorization;

namespace gamitude_backend.IntegrationTests
{
    [CollectionDefinition("Projects collection")]
    public class ProjectsCollection : ICollectionFixture<WebApplicationFactory<gamitude_backend.Startup>> { }
    //IClassFixture<T> for context per test 
    // example IClassFixture<WebApplicationFactory<gamitude_backend.Startup>> set database and server per tets
    // ICollectionFixture<T> per collection [Collection("Database collection")]
    [Collection("Projects collection")]
    public class ProjectsTests : IDisposable
    {
        private readonly WebApplicationFactory<gamitude_backend.Startup> _factory;
        private readonly String _email = "test@test.pl";
        private readonly String _userName = "test";
        private readonly String _password = "Asdf1234";

        private MongoIntegrationTest mongo = null;

        public void Dispose()
        {
            if (mongo != null)
            {
                mongo._runner.Dispose();
            }
        }

        public ProjectsTests(WebApplicationFactory<gamitude_backend.Startup> factory)
        {
            mongo = new MongoIntegrationTest();

            mongo.CreateDatabase();
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, conf) =>
                {
                    Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
                    conf.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        ["DatabaseSettings:ConnectionString"] = mongo._runner.ConnectionString + "gamitude"
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
        [Fact]
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
            }.ToExpectedObject();

            // Act
            var response = await client.PostAsJsonAsync("/api/users", user);
            var contentString = await response.Content.ReadAsStringAsync();
            var actualResponse = System.Text.Json.JsonSerializer.Deserialize<ControllerResponse<GetUserDto>>(contentString);
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            expectedResponse.ShouldEqual(actualResponse);

            var login = new LoginUserDto
            {
                login = _userName,
                password = _password,
            };

            // Act
            var response2 = await client.PostAsJsonAsync("/api/authorization/login", login);
            var contentString2 = await response2.Content.ReadAsStringAsync();
            var actualResponse2 = System.Text.Json.JsonSerializer.Deserialize<ControllerResponse<GetUserTokenDto>>(contentString2);
            // Assert
            response2.EnsureSuccessStatusCode(); // Status Code 200-299
            // Assert.Equal(_email, actualResponse2.data.user.email);
            // Assert.Equal(_userName, actualResponse2.data.user.userName);
            Assert.True(actualResponse2.success);
        }
        [Fact]
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
            Assert.Equal(_email, actualResponse.data.user.email);
            Assert.Equal(_userName, actualResponse.data.user.userName);
            Assert.True(actualResponse.success);
        }
    }

}