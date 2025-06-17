using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {       
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductRepository _prodRepo;

        public ProductsController(ILogger<ProductsController> logger, IProductRepository prodRepo)
        {
            _logger = logger;
            _prodRepo = prodRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? color)
        {
            try
            {
                IEnumerable<Product> prods;

                if (string.IsNullOrEmpty(color) || string.IsNullOrWhiteSpace(color))
                {
                    prods = await _prodRepo.GetAll();
                }
                else
                {
                    prods = await _prodRepo.GetAll();
                }

                if (!prods.Any())
                    return NotFound();

                return Ok(prods);
            }
            catch (Exception ex)
            {
                var err = $"Get Product Exception: {ex.Message} | InnerException: {ex.InnerException} ";
                _logger.LogError(err, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }           
        }
       
        [HttpPost]
        public async Task<IActionResult> Add(Product model)
        {
            if (model == null)
                return BadRequest("Product cannot be null");

            try
            {
                var prod = await _prodRepo.Add(model);
                return Ok(prod);
            }
            catch (Exception ex)
            {
                var err = $"Add Product Exception: {ex.Message} | InnerException: {ex.InnerException} ";
                _logger.LogError(err, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }
        }

       
    }
}
