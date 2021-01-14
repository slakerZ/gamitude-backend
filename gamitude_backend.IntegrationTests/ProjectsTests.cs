using System;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using gamitude_backend.Dto.User;
using System.Text.Json;
using System.Net.Http.Json;
using gamitude_backend.Dto;

namespace gamitude_backend.IntegrationTests
{
    public class ProjectsTests : MongoIntegrationTest, IClassFixture<CustomWebApplicationFactory<gamitude_backend.Startup>>
    {
        private readonly CustomWebApplicationFactory<gamitude_backend.Startup> _factory;

        public ProjectsTests(CustomWebApplicationFactory<gamitude_backend.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/api/version")]
        public async void Test1(string url)
        {
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async void CreateUser()
        {
            CreateDatabase();
            var client = _factory.CreateClient();
            // Act
            var user = new CreateUserDto
            {
                email = "test@test.pl",
                password = "Asdf1234",
                userName = "test"
            };
            var userResponse = new GetUserDto
            {
                email = user.email,
                userName = user.userName
            };
            var validResponse = new ControllerResponse<GetUserDto>
            {
                success = true,
                data = userResponse
            };

            var response = await client.PostAsJsonAsync("/api/users", user);
            var contentString = await response.Content.ReadAsStringAsync();
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(validResponse,
                 JsonSerializer.Deserialize<ControllerResponse<GetUserDto>>(((contentString))));
        }

    }

}