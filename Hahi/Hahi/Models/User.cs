using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hahi.Models
{
    public partial class User
    {
        public User()
        {
            Requests = new HashSet<Request>();
        }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int? AccountId { get; set; }
        [Required]
        public int? RoleId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public virtual Account? Account { get; set; }
        [Required]
        public virtual Role? Role { get; set; }
        [Required]
        public virtual ICollection<Request> Requests { get; set; }
    }
}
