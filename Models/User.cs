using System;
using System.Collections.Generic;

namespace KoiPond.Models;

public partial class User
{
    public int UserId { get; set; }

    public int? AccountId { get; set; }

    public int RoleId { get; set; }

    public string? Name { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual Role? Role { get; set; }
}
