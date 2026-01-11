using StockApi.Modals;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using VidepGame.Data;
using StockApi.Dtos;

namespace StockApi.Services
{
    public class ProductService(AppDbContext context): IProductService
    {

        public async Task<List<Product>> GetProductsAsync()
        {
            return await context.products.ToListAsync();
        }

        public async Task<Product> AddProductAsync(ProductCreateDto newProduct)
        {
            var product = new Product
            {
                Name = newProduct.Name,
                UnitMl = newProduct.UnitMl,
                TareWeightG = newProduct.TareWeightG,
                LossAllowance = newProduct.LossAllowance,
            };

            context.products.Add(product);
            await context.SaveChangesAsync();

            return product;
        }

        public async Task<Product?> GetProductAsync(int id) { 
        var result = await context.products.FindAsync(id);
            return result;
        }

        public async Task<bool> UpdateProductAsync(int id, ProductUpdateDto product)
        {
            var foundProduct = await context.products.FindAsync(id);

            if(foundProduct is null)
            {
                return false;
            }

            foundProduct.Name = product.Name;
            foundProduct.UnitMl = product.UnitMl;
            foundProduct.TareWeightG = product.TareWeightG;
            foundProduct.LossAllowance = product.LossAllowance;
            foundProduct.SellingPrice = product.SellingPrice;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var foundProduct = await context.products.FindAsync(id);
            if(foundProduct is null)
            {
                return false;
            }

             context.products.Remove(foundProduct);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
