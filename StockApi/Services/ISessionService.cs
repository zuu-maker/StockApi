using StockApi.Dtos;
using StockApi.Modals;

namespace StockApi.Services
{
    public interface ISessionService
    {

        Task<List<Session>> GetSessionsAsync();
        Task<Session?> GetSessionAsync(int id);
        Task<Session> AddSessionAsync(SessionCreateDto newSession);
        Task<bool> UpdateSessionAsync(int id,SessionUpdateDto session);
        Task<bool> DeleteSessionAsync(int id);

    }
}
