using System;
using System.Collections.Generic;

namespace Hahi.ModelsV1
{
    public partial class User
    {
        public User()
        {
            Requests = new HashSet<Request>();
        }

        public int UserId { get; set; }
        public int? AccountId { get; set; }
        public int RoleId { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

        public virtual Account? Account { get; set; }
        public virtual Role? Role { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
    }
}
