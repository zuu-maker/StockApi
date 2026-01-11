using System;
using StockApi.Dtos;
using StockApi.Modals;


namespace StockApi.Services
{
    public interface IProductService
    {

        Task<List<Product>> GetProductsAsync();
        Task<Product?> GetProductAsync(int id);
        Task<Product> AddProductAsync(ProductCreateDto newProduct);
        Task<bool> UpdateProductAsync(int id, ProductUpdateDto updatedProduct);  
        Task<bool> DeleteProductAsync (int id);


    }
}
