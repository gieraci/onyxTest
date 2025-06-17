using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using WebApi.Models;

namespace Test
{
    public class IntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public IntegrationTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task HealthCheck_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/health");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Healthy", content);
        }

        [Fact]
        public async Task GetAllProducts_RequiresAuth()
        {
            // Act
            var response = await _client.GetAsync("/products");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task CreateProduct_UnauthorizedWithoutToken()
        {
            // Act
            var response = await _client.PostAsJsonAsync("/products", new Product
            {
                Name = "Unauthorized Racket",
                Color = "Gray",
                Price = 199
            });

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
    
}
