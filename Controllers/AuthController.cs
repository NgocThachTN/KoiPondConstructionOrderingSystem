using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using KoiPond.Models;
using Google.Apis.Auth;
// For async database calls

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly KoiContext _context;
    private readonly string _googleClientId;
    private readonly IConfiguration _configuration;

    public AuthController(KoiContext context, IConfiguration configuration)
    {
        _configuration = configuration;
        _context = context;
        _googleClientId = _configuration["GoogleOAuth:ClientId"];

    }



    [HttpPost("google")]
    public async Task<IActionResult> GoogleLogin([FromBody] string token)
    {
            var payload = await GoogleJsonWebSignature.ValidateAsync(token, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _googleClientId }

            });
            var jwtToken = GenerateJwtToken(payload.Email, payload.Name, payload.Picture);

            return Ok(jwtToken = token);
    }

    [HttpPost("generate-jwt")]
    public string GenerateJwtToken(string email, string name, string picture)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Name, name),
            new Claim("picture", picture)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_jwt_secret_key_here I hate this coding section please kill yourself now1231231231232"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        var account = await _context.Accounts
            .Include(a => a.User) // Include the User relationship
                .ThenInclude(u => u.Role) // Include the Role from the User
            .FirstOrDefaultAsync(a => a.Email == login.Email && a.Password == login.Password);

        if (account != null)
        {
            var user = account.User;
            var role = user?.Role?.RoleName ?? "Unknown";

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("your_jwt_secret_key_here I hate this coding section please kill yourself now1231231231232");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, account.Email),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Audience = "your_audience_here",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Token = tokenString,
                Email = account.Email,
                Role = role
            });
        }

        return Unauthorized();
    }

    [HttpPost("signup")]
    [SwaggerRequestExample(typeof(SignupModel), typeof(SignupModelExample))] // Adding example for Swagger
    public async Task<IActionResult> Signup([FromBody] SignupModel signup)
    {
        var existingAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.UserName == signup.Username);
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
            UserName = signup.Username,
            Email = signup.Email,
            Password = signup.Password
        };

        var newUser = new User
        {
            Name = signup.Name,
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

// Example for Swagger request body
public class SignupModelExample : Swashbuckle.AspNetCore.Filters.IExamplesProvider<SignupModel>
{
    public SignupModel GetExamples()
    {
        return new SignupModel
        {
            Username = "default_user",
            Password = "DefaultPassword123",
            Email = "defaultemail@example.com",
            Name = "Default Name",
            PhoneNumber = "123456789",
            Address = "123 Default St, City",
            RoleId = 1 // Will default to Customer role
        };
    }
}
public class LoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }


}

public class SignupModel
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public int RoleId { get; set; }
}
