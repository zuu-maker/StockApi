using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace StockApi.Services
{
    public class AuthService(IConfiguration configuration) : IAuthService
    {
        public string GenerateToken(string employeeId, string role)
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, employeeId),
                new Claim(ClaimTypes.Name, employeeId),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            var hashOfInput = HashPassword(password);
            Console.WriteLine(hashOfInput);
            //Logger.LogInformation("Hello from minimal API");
            return hashOfInput == passwordHash;
        }

        public string GenerateEmployeeId(string employeeType, int currentYear, int counter)
        {
            // Format: WR2026001, WR2026002, MG2026001, etc.
            return $"{employeeType.ToUpper()}{currentYear}{counter:D3}";
        }
    }
}
