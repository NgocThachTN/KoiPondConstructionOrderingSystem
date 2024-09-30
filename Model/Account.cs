using KoiPondConstructionManagement.Data;
using Microsoft.AspNetCore.Identity;

namespace KoiPondConstructionManagement.Model;

public partial class Account
{
    public int AccountId { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public virtual User? User { get; set; }

}
