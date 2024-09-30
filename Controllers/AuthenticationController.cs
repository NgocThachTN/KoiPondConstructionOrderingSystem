
using KoiPondConstructionManagement.Data;
using KoiPondConstructionManagement.Interface;
using KoiPondConstructionManagement.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoiPondConstructionManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {

        private readonly PrototypeKoiv1Context _context;

        private readonly ITokenService _tokenService;

        public AuthenticationController(PrototypeKoiv1Context context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] SignInModel model)
        {
            var account = await _context.Accounts
                .Include(a => a.User) // Include the User relationship
                    .ThenInclude(u => u.Role) // Include the Role from the User
                .FirstOrDefaultAsync(a => a.Email == model.Email && a.Password == model.Password);

            if (account != null)
            {
                var user = account.User;
                var role = user?.Role?.RoleName ?? "Unknown";

                return Ok(new
                {
                    Token = _tokenService.CreateTokenUser(account),
                    Email = account.Email,
                    Role = role
                });
            }

            return Unauthorized();
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignUpModel signup)
        {
            var existingAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.UserName == signup.UserName);
            if (existingAccount != null)
            {
                return BadRequest(new { Message = "Username already exists" });
            }

            var defaultCustomerRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Customer");
            if (defaultCustomerRole == null)
            {
                return BadRequest(new { Message = "Customer role not found" });
            }

            var newAccount = new Account
            {
                UserName = signup.UserName,
                Email = signup.Email,
                Password = signup.Password
            };

            var newUser = new User
            {
                Name = signup.UserName,
                PhoneNumber = signup.PhoneNumber,
                Address = signup.Address,
                RoleId = signup.RoleId > 1 ? signup.RoleId : defaultCustomerRole.RoleId,
                Account = newAccount
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Signup successful" });
        }
    }
}
