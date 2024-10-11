using System.ComponentModel.DataAnnotations;

namespace KoiPond.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public int? AccountId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int RoleId { get; set; } = 1;
    }
}