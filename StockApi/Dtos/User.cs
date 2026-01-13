using System.ComponentModel.DataAnnotations;

namespace StockApi.Dtos
{
    public class User
    {
        public class CreateUserDto
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [StringLength(100)]
            public string FullName { get; set; } = string.Empty;

            [Required]
            [StringLength(10)]
            public string EmployeeType { get; set; } = string.Empty;

            [Required]
            [StringLength(100, MinimumLength = 6)]
            public string InitialPassword { get; set; } = string.Empty;

            [StringLength(20)]
            public string Role { get; set; } = "User";
        }

        public class UserLoginDto
        {
            [Required]
            public string EmployeeId { get; set; } // e.g., WR2026001

            [Required]
            public string Password { get; set; }
        }

        public class UserUpdateDto
        {
            [EmailAddress]
            public string Email { get; set; }

            [StringLength(100)]
            public string FullName { get; set; }

            public bool? IsActive { get; set; }

            [StringLength(20)]
            public string Role { get; set; }
        }

        public class ChangePasswordDto
        {
            [Required]
            public string CurrentPassword { get; set; }

            [Required]
            [StringLength(100, MinimumLength = 6)]
            public string NewPassword { get; set; }

            [Required]
            [Compare("NewPassword")]
            public string ConfirmNewPassword { get; set; }
        }

        public class UserResponseDto
        {
            public string EmployeeId { get; set; }
            public string Email { get; set; }
            public string FullName { get; set; }
            public string EmployeeType { get; set; }
            public string Role { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? LastLoginAt { get; set; }
        }

        public class AuthResponseDto
        {
            public string Token { get; set; }
            public UserResponseDto User { get; set; }
        }
    }
}
