using Azure.Core;
using Microsoft.EntityFrameworkCore;
using StockApi.Dtos;
using StockApi.Modals;
using System.Security.Cryptography.X509Certificates;
using VidepGame.Data;


namespace StockApi.Services
{
    public class StockLinesService(AppDbContext context): IStockLinesService
    {

        public async Task<StockLine> AddStockLineAsync(StockLineCreateDto stockLine)
        {

            var foundProduct = await context.products.FindAsync(stockLine.ProductId);

            if(foundProduct is null)
            {
                throw new Exception("Product not found");
            }
            // 2. RUN THE CALCULATOR NOW
            var result = Calculator.Calculate(
                foundProduct.UnitMl,
                foundProduct.TareWeightG,
                stockLine.FullGrossG,
                stockLine.CurrentGrossG,
                20,
                foundProduct.LossAllowance
            );

            Console.WriteLine(result);

            var newStockLine = new StockLine
            {
                SessionId = stockLine.SessionId,
                ProductId = stockLine.ProductId,
                ProductionName = stockLine.ProductionName,
                ProductUnitMl = foundProduct.UnitMl,
                UnitOfMeasure = stockLine.UnitOfMeasure,
                FullGrossG = stockLine.FullGrossG,
                CurrentGrossG = stockLine.CurrentGrossG,
                RemainingVolumeMl = result.RemainingMl,
                RemainingServingsExact = result.ExactShots,
                RemainngServingsWhole = result.WholeShots,
                SellingPrice = foundProduct.SellingPrice,
                LineValue = result.WholeShots * foundProduct.SellingPrice,
                CreatedAt = stockLine.CreatedAt
            };

            context.stockLines.Add(newStockLine);
            await context.SaveChangesAsync();
            return newStockLine;
        }

        public async Task<List<StockLine>> GetStockLinesAsync()
        {
            var stockLines = await  context.stockLines.ToListAsync();
            return stockLines;
        }

        public async Task<List<StockLine>> GetMyStockLines(int sessionId)
        {
            var stockLines = await context.stockLines.Where(sl => sl.SessionId == sessionId).ToListAsync();
            return stockLines;
        }

        public async Task<StockLine?> GetStockLineAsync(int id)
        {
            var stockLine = await context.stockLines.FindAsync(id);

            return stockLine;
        }

        public async Task<bool> UpdateStockLineAsync(int id, StockLineUpdateDto stockLine)
        {
            var foundStockLine = await context.stockLines.FindAsync(id);

            if(foundStockLine is null)
            {
                return false;
            }

            foundStockLine.SessionId = stockLine.SessionId; 
            foundStockLine.ProductId = stockLine.ProductId;
            foundStockLine.ProductionName = stockLine.ProductionName;
            foundStockLine.ProductUnitMl = stockLine.ProductUnitMl;
            foundStockLine.UnitOfMeasure = stockLine.UnitOfMeasure;
            foundStockLine.FullGrossG = stockLine.FullGrossG;
            foundStockLine.CurrentGrossG = stockLine.CurrentGrossG;
            foundStockLine.RemainingVolumeMl = stockLine.RemainingVolumeMl;
            foundStockLine.RemainingServingsExact = stockLine.RemainingServingsExact;
            foundStockLine.RemainngServingsWhole = stockLine.RemainngServingsWhole;
            foundStockLine.SellingPrice = stockLine.SellingPrice;
            foundStockLine.LineValue = stockLine.LineValue;



            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteStockLineAsync(int id)
        {
            var foundStockLine = await context.stockLines.FindAsync(id);

            if(foundStockLine is null)
            {
                return false;
            }

            context.stockLines.Remove(foundStockLine);
            await context.SaveChangesAsync();
            return true;
        }

    }

      
}
