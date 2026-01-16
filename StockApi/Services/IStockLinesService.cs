using StockApi.Modals;
using StockApi.Dtos;

namespace StockApi.Services
{
    public interface IStockLinesService
    {
        Task<List<StockLine>> GetStockLinesAsync();
        Task<List<StockLine>> GetMyStockLines(int sessionId);
        Task<StockLine?> GetStockLineAsync(int id);
        Task<StockLine> AddStockLineAsync(StockLineCreateDto newStockLine);
        Task<bool> DeleteStockLineAsync(int id);
        Task<bool> UpdateStockLineAsync(int id, StockLineUpdateDto stockLine);
    }
}
