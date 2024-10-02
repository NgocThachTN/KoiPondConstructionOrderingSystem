using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hahi.Models
{
    public partial class User
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int AccountId { get; set; }  // This links the user to an account

        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        // Role that this user is associated with
        [Required]
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        // Requests made by the user
        public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

        // Reference back to the account (needed to avoid circular reference issues)
        public virtual Account Account { get; set; }
    }

}
