using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hahi.Models
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int RoleId { get; set; }
        [Required]
        public string? RoleName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
