using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hahi.ModelsV1;
using Hahi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Hahi.AutoMapper;
using Hahi.DTOs;

namespace Hahi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _repository;
        private readonly KoisV1Context _context;

        public UsersController(IUsersRepository repository, KoisV1Context context)
        {
            _repository = repository;
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _repository.GetUsersAsync();
            var requestDtos = users.Select(user => user.ToUserDto()).ToList(); // Using RequestMapper
            return Ok(requestDtos);
        }

        // GET: api/Users/5
        [HttpGet("{UserId}")]
        public async Task<ActionResult<User>> GetUserById(int UserId)
        {
            var user = await _repository.GetUserByIdAsync(UserId);

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            try
            {
                return Ok(user.ToUserDto());
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message }); // Return NotFound if Account is null
            }
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public IActionResult UpdateUser([FromRoute] int id, [FromBody] UpdateUserRequestDto modelUpdate)
        {
            var userModel = _context.Users.FirstOrDefault(x => x.UserId == id);
            if (userModel == null)
            {
                return NotFound();
            }

            if (userModel.Account == null)
            {
                userModel.Account = new Account();
            }

            userModel.Account.UserName = modelUpdate.UserName;
            userModel.Account.Email = modelUpdate.Email;
            userModel.Account.Password = modelUpdate.Password;
            userModel.Name = modelUpdate.Name;
            userModel.Address = modelUpdate.Address;
            userModel.PhoneNumber = modelUpdate.PhoneNumber;
            userModel.RoleId = modelUpdate.RoleId;

            _context.SaveChanges();

            return Ok(userModel.ToUserDto());
        }

        // POST: api/Users
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public IActionResult CreateUser([FromBody] CreateUserRequestDto user)
        {
            var userModel = user.ToUserFromCreatedDto();
            _context.Users.Add(userModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetUserById), new { UserID = userModel.UserId }, userModel.ToUserDto());
        }


        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Retrieve the user including their associated Account
            var user = await _repository.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            var relatedRequests = await _context.Requests
                .Where(r => r.UserId == id)
                .ToListAsync();

            if (relatedRequests.Any())
            {
                _context.Requests.RemoveRange(relatedRequests);
            }

            // Check if the associated Account exists
            if (user.Account == null)
            {
                return NotFound(new { message = "Account ID not found." });
            }

            if (user.Role == null)
            {
                return NotFound(new { message = "Role ID not found." });
            }

            // Proceed to delete the user
            bool result = await _repository.DeleteUserAsync(id);

            if (result)
            {
                return Ok(new { message = "User deleted successfully." });
            }

            return BadRequest(new { message = "Failed to delete user." });
        }

    }
}

