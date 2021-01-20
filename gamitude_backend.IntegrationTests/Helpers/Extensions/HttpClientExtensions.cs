

using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using gamitude_backend.Dto;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace gamitude_backend.IntegrationTests.Extensions
{
    public static class HttpClientExtension
    {
        public async static Task<ControllerResponse<T>> testSuccessGetAsync<T>(this HttpClient client,string url)
        {
            var response = await client.GetAsync(url);
            var contentString = await response.Content.ReadAsStringAsync();
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var actualResponse = JsonConvert.DeserializeObject<ControllerResponse<T>>(contentString);            
            Assert.True(actualResponse.success);
            
            return actualResponse;
        }

        public async static Task<ControllerResponse<T>> testSuccessPostAsync<T,L>(this HttpClient client,string url,L body)
        {
            var response = await client.PostAsJsonAsync(url, body);
            var contentString = await response.Content.ReadAsStringAsync();
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var actualResponse = JsonConvert.DeserializeObject<ControllerResponse<T>>(contentString);            
            Assert.True(actualResponse.success);
            
            return actualResponse;
        }

        public async static Task<ControllerResponse<T>> testSuccessPutAsync<T,L>(this HttpClient client,string url,L body)
        {
            var response = await client.PutAsJsonAsync(url, body);
            var contentString = await response.Content.ReadAsStringAsync();
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var actualResponse = JsonConvert.DeserializeObject<ControllerResponse<T>>(contentString);            
            Assert.True(actualResponse.success);
            
            return actualResponse;
        }

        public async static Task testSuccessDeletAsync(this HttpClient client,string url)
        {
            var response = await client.DeleteAsync(url);
            var contentString = await response.Content.ReadAsStringAsync();
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299            
        }
    }
}