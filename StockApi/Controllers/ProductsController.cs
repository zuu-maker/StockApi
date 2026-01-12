using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockApi.Dtos;
using StockApi.Modals;
using StockApi.Services;

namespace StockApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var result = await service.GetProductsAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> getProduct(int id)
        {
            var result = await service.GetProductAsync(id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> deleteProduct(int id)
        {
            var result = await service.DeleteProductAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> addProduct(ProductCreateDto newProduct)
        {
            var result = await service.AddProductAsync(newProduct);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> updateProduct(int id, ProductUpdateDto product)
        {
            var result = await service.UpdateProductAsync(id, product);
            return Ok(result);
        }
        
    }
}
