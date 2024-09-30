using System.ComponentModel.DataAnnotations;

namespace KoiPondConstructionManagement.Data
{
    public class SignUpModel
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; } = null!;

        [Required]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")]
        public string ConfirmPassword { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public int RoleId { get; set; }

    }
}
