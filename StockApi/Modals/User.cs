using System.ComponentModel.DataAnnotations;

namespace StockApi.Modals
{
    public class User
    {
        [Key]
        [StringLength(20)]
        public string EmployeeId { get; set; } // e.g., WR2026001, WR2026002

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(10)]
        public string EmployeeType { get; set; } // WR (Worker), MG (Manager), etc.

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLoginAt { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(20)]
        public string Role { get; set; } = "User"; // User, Admin, etc.
    }
}
