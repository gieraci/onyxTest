using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Models;
using WebApi.Repositories;

namespace Test
{
    public class ProductRepositoryTest
    {
        private readonly ProductRepository _repo;
        public ProductRepositoryTest()
        {
            var mockLogger = new Mock<ILogger<ProductRepository>>();
            _repo = new ProductRepository(mockLogger.Object);
        }
       
        [Fact]
        public async Task Add_ShouldAssignIdAndStoreProduct()
        {
            // Arrange
            var product = new Product { Name = "Wilson Pro Staff", Color = "Green", Price = 123 };

            // Act
            var result = await _repo.Add(product);
            var allProducts = await _repo.GetAll();

            // Assert
            Assert.True(result.Id>0);
            Assert.Contains(allProducts, p => p.Name == "Wilson Pro Staff" && p.Color == "Green" && p.Price==123);
        }

        [Fact]
        public async Task GetByColor_ShouldReturnCorrectProducts()
        {
            // Arrange
            var redProduct = new Product { Name = "Wilson Pro Staff", Color = "Red", Price = 200 };
            await _repo.Add(redProduct);

            // Act
            var results = await _repo.GetByColor("Red");

            // Assert
            Assert.NotNull(results);
            Assert.True(results.Any());
            Assert.All(results,p=> Assert.Equal("Red",p.Color,ignoreCase:true));       
        }

        [Fact]
        public async Task GetByColor_WithUnknownColor_ShouldReturnEmpty()
        {
            // Act
            var results = await _repo.GetByColor("clear");

            // Assert
            Assert.NotNull(results);
            Assert.Empty(results);
  
        }
    }
}