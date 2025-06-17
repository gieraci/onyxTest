using System.Collections.Concurrent;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class ProductRepository : IProductRepository
    {

        private readonly ConcurrentDictionary<int, Product> _products = new ConcurrentDictionary<int, Product>();
        private readonly ILogger<ProductRepository> _logger;
        private int _nextId = 0;
       
        public ProductRepository(ILogger<ProductRepository> logger) {
            
            _logger = logger;

            Add(new Product { Name = "Babolat Pure Aero", Color = "Yellow" , Price = 280 });
            Add(new Product { Name = "Yonex Ezone 100", Color = "Blue", Price = 250 });
            Add(new Product { Name = "Wilson Ultra V4", Color = "Navy", Price = 180 });
            Add(new Product { Name = "Head Speed Elite", Color = "Black", Price = 220 });
        }
        public Task<Product> Add(Product product)
        {
            try
            {               
                product.Id=Interlocked.Increment(ref _nextId);
                if (_products.TryAdd(product.Id,product))
                {
                    _logger.LogInformation($"New Product Added - {product.ToString()}");
                    return Task.FromResult(product);
                }
                else
                {
                    throw new InvalidOperationException("Failed to add product to store.");
                }
               
            }
            catch (Exception ex)
            {
                _logger.LogError($" ProductRepository - Add : failed to add product : {product.Name} - {ex}");
                throw new Exception(" ProductRepository - Add :failed to add product ", ex);
            }

        }

        public Task<IEnumerable<Product>> GetAll()
        {
            try
            {
                _logger.LogInformation($" ProductRepository - GetAll");
                return Task.FromResult<IEnumerable<Product>>(_products.Values.ToList());
              
            }
            catch (Exception ex)
            {
               _logger.LogError($" ProductRepository - GetAll : failed to retrieve all products. {ex}");
               throw new Exception(" ProductRepository - GetAll : failed to retrieve all products.", ex);
              
            }
        }

        public Task<IEnumerable<Product>> GetByColor(string color)
        {
            try
            {
               
                _logger.LogInformation($" ProductRepository - GetByColor : {color}");

                if (string.IsNullOrEmpty(color) || string.IsNullOrWhiteSpace(color))
                    return Task.FromResult<IEnumerable<Product>>(Enumerable.Empty<Product>());

                var itemsfilterd = _products.Values.Where(p => p.Color.Equals(color, StringComparison.OrdinalIgnoreCase)).ToList();
                
                return Task.FromResult<IEnumerable<Product>>(itemsfilterd);
                
            }
            catch (Exception ex)
            {
                _logger.LogError($" ProductRepository - GetAll : failed to retrieve all products. {ex}");
                throw new Exception(" ProductRepository - GetAll : failed to retrieve all products.", ex);

            }
        }

       
    }
}
