namespace StockApi.Services
{
    public interface IAuthService
    {
        string GenerateToken(string employeeId, string role);
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
        string GenerateEmployeeId(string employeeType, int currentYear, int counter);

    }
}
