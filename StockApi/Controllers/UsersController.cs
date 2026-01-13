using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockApi.Modals;
using StockApi.Services;
using System.Security.Claims;
using VidepGame.Data;
using static StockApi.Dtos.User;

//it makes a lot more sense to have the database actions directly in the conrollers because you can control the responses

namespace StockApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(AppDbContext context, IAuthService service) : ControllerBase
    {

        private UserResponseDto MapToUserResponse(User user)
        {
            return new UserResponseDto
            {
                EmployeeId = user.EmployeeId,
                Email = user.Email,
                FullName = user.FullName,
                EmployeeType = user.EmployeeType,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
            };
        }

        private bool UserExists(string id)
        {
            return context.users.Any(e => e.EmployeeId == id.ToUpper());
        }

        private string GetCurrentEmployeeId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> CreateUser(CreateUserDto dto)
        {
            if (await context.users.AnyAsync(u => u.Email == dto.Email))
            {
                return BadRequest(new { message = "Email already exists" });
            }

            // Get the current year
            int currentYear = DateTime.Now.Year;

            // Get the count of users with the same employee type in the current year
            var employeeTypePrefix = $"{dto.EmployeeType.ToUpper()}{currentYear}";
            var existingUsersCount = await context.users
                .Where(u => u.EmployeeId.StartsWith(employeeTypePrefix))
                .CountAsync();

            // Generate new employee ID
            int counter = existingUsersCount + 1;
            var employeeId = service.GenerateEmployeeId(dto.EmployeeType, currentYear, counter);

            var user = new User
            {
                EmployeeId = employeeId,
                Email = dto.Email,
                PasswordHash = service.HashPassword(dto.InitialPassword),
                FullName = dto.FullName,
                EmployeeType = dto.EmployeeType.ToUpper(),
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Role = dto.Role ?? "User"
            };

            context.users.Add(user);
            await context.SaveChangesAsync();

            // go through created action
            return Ok(user);
                
                //CreatedAtAction(nameof(GetUser), new { id = user.EmployeeId },
                //new
                //{
                //    message = "User created successfully",
                //    employeeId = user.EmployeeId,
                //    initialPassword = dto.InitialPassword, // Return this once so admin can give it to the user
                //    user = MapToUserResponse(user)
                //});
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(UserLoginDto dto)
        {
            var user = await context.users
                .FirstOrDefaultAsync(u => u.EmployeeId == dto.EmployeeId.ToUpper());

            if (user == null || !service.VerifyPassword(dto.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid employee ID or password" });
            }

            if (!user.IsActive)
            {
                return Unauthorized(new { message = "Account is inactive" });
            }

            // Update last login
            user.LastLoginAt = DateTime.UtcNow;
            await context.SaveChangesAsync();

            var token = service.GenerateToken(user.EmployeeId, user.Role);

            return Ok(new AuthResponseDto
            {
                Token = token,
                User = MapToUserResponse(user)
            });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers(
            string? employeeType = null,
            bool? isActive = null)
        {
            var query = context.users.AsQueryable();

            // Filter by employee type if provided
            if (!string.IsNullOrEmpty(employeeType))
            {
                query = query.Where(u => u.EmployeeType == employeeType.ToUpper());
            }

            // Filter by active status if provided
            if (isActive.HasValue)
            {
                query = query.Where(u => u.IsActive == isActive.Value);
            }

            var users = await query
                .OrderBy(u => u.EmployeeId)
                .ToListAsync();

            var userResponses = users.Select(u => MapToUserResponse(u)).ToList();

            return Ok(userResponses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUser(string id)
        {

            // what is this gooinh
            //var currentEmployeeId = GetCurrentEmployeeId();
            //var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            //// Users can only view their own profile unless they're admin
            //if (currentEmployeeId != id.ToUpper() && currentUserRole != "Admin")
            //{
            //    return Forbid();
            //}

            var user = await context.users.FindAsync(id.ToUpper());

            if (user == null)
            {
                return NotFound();
            }

            return Ok(MapToUserResponse(user));
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UserResponseDto>> GetCurrentUser()
        {
            var employeeId = GetCurrentEmployeeId();
            var user = await context.users.FindAsync(employeeId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(MapToUserResponse(user));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(string id, UserUpdateDto dto)
        {
            var currentEmployeeId = GetCurrentEmployeeId();
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Users can only update their own profile unless they're admin
            if (currentEmployeeId != id.ToUpper() && currentUserRole != "Admin")
            {
                return Forbid();
            }

            var user = await context.users.FindAsync(id.ToUpper());

            if (user == null)
            {
                return NotFound();
            }

            // Update fields if provided
            if (!string.IsNullOrEmpty(dto.Email))
            {
                // Check if email is already taken by another user
                if (await context.users.AnyAsync(u => u.Email == dto.Email && u.EmployeeId != id.ToUpper()))
                {
                    return BadRequest(new { message = "Email already exists" });
                }
                user.Email = dto.Email;
            }

            if (!string.IsNullOrEmpty(dto.FullName))
            {
                user.FullName = dto.FullName;
            }

            // Only admins can change these
            if (currentUserRole == "Admin")
            {
                if (dto.IsActive.HasValue)
                {
                    user.IsActive = dto.IsActive.Value;
                }

                if (!string.IsNullOrEmpty(dto.Role))
                {
                    user.Role = dto.Role;
                }
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpPost("{id}/change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string id, [FromBody] ChangePasswordDto dto)
        {
            var currentEmployeeId = GetCurrentEmployeeId();

            // Users can only change their own password
            if (currentEmployeeId != id.ToUpper())
            {
                return Forbid();
            }

            var user = await context.users.FindAsync(id.ToUpper());

            if (user == null)
            {
                return NotFound();
            }

            // Verify current password
            if (!service.VerifyPassword(dto.CurrentPassword, user.PasswordHash))
            {
                return BadRequest(new { message = "Current password is incorrect" });
            }

            // Update password
            user.PasswordHash = service.HashPassword(dto.NewPassword);
            await context.SaveChangesAsync();

            return Ok(new { message = "Password changed successfully" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await context.users.FindAsync(id.ToUpper());
            if (user == null)
            {
                return NotFound();
            }

            context.users.Remove(user);
            await context.SaveChangesAsync();

            return NoContent();
        }

    }
}
