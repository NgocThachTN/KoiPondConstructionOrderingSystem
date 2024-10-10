using System.ComponentModel.DataAnnotations;

namespace KoiPond.DTOs
{
    public class CreateAccountRequestDto
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
        public int RoleId { get; set; } = 1;
    }
}
