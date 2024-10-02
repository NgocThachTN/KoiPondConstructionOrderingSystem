using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hahi.Models
{
    public partial class Account
    {
        public int AccountId { get; set; }

        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        // Account is linked to one User
        public virtual User User { get; set; }
    }

}
