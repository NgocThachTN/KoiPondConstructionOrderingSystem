using System.ComponentModel.DataAnnotations;

namespace Hahi.DTOs
{
    public class CreateAccountRequestDto
    {
        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
