using System.ComponentModel.DataAnnotations;

namespace LoanManagementSystem.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(255, ErrorMessage = "Email cannot be longer than 255 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
