using WebApi.Models;

namespace WebApi.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAll();
        Task<IEnumerable<Product>> GetByColor(string color);
        Task<Product> Add(Product product);
    }
}
