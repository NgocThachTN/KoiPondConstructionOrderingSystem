using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using KoiPond.Models;
using KoiPond.DTOs;
using KoiPond.Repositories;
using KoiPond.AutoMapper;


namespace KoiPond.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsRepository _repository;
        private readonly KoiContext _context;

        public AccountsController(IAccountsRepository repository, KoiContext context)
        {
            _context = context;
            _repository = repository;
        }

        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccounts()
        {
            var accounts = await _repository.GetAccounts()
                                            .Include(a => a.User) // Ensure related User data is loaded
                                            .Where(account => account.User != null)
                                            .ToListAsync();

            var accountDtos = accounts.Select(account => account.ToAccountDto())
                                      .Where(dto => dto != null)
                                      .ToList();

            return Ok(accountDtos);
        }

        // GET: api/Accounts/5
        [HttpGet("{AccountId}")]
        public async Task<ActionResult<AccountDto>> GetAccountById(int AccountId)
        {
            var account = await _repository.GetAccountByIdAsync(AccountId);

            if (account == null)
            {
                return NotFound(new { message = "Account not found." });
            }

            try
            {
                return Ok(account.ToAccountDto());
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message }); // Return NotFound if Account is null
            }
        }


        // PUT: api/Accounts/5
        [HttpPut("{id}")]
        public IActionResult UpdateAccount([FromRoute] int id, [FromBody] UpdateAccountRequestDto modelUpdate)
        {
            // Include the User entity when fetching the Account
            var accountModel = _context.Accounts.Include(a => a.User).FirstOrDefault(x => x.AccountId == id);

            if (accountModel == null)
            {
                return NotFound();
            }

            // Check if User is null and initialize it if necessary
            if (accountModel.User == null)
            {
                accountModel.User = new User(); // Initialize the User entity if it's null
            }

            // Update account and user details
            accountModel.UserName = modelUpdate.UserName;
            accountModel.Email = modelUpdate.Email;
            accountModel.Password = modelUpdate.Password;
            accountModel.User.Name = modelUpdate.Name;
            accountModel.User.PhoneNumber = modelUpdate.PhoneNumber;
            accountModel.User.Address = modelUpdate.Address;
            accountModel.User.RoleId = modelUpdate.RoleId;

            // Save changes to the database
            _context.SaveChanges();

            // Return the updated account as a DTO
            return Ok(accountModel.ToAccountDto());
        }




        // POST: api/Accounts
        [HttpPost]
        [Authorize]
        [HttpPost]
        public IActionResult CreateAccount([FromBody] CreateAccountRequestDto account)
        {
            var accountModel = account.ToAccountFromCreatedDto();
            _context.Accounts.Add(accountModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetAccountById), new { AccountID = accountModel.AccountId }, accountModel.ToAccountDto());
        }


        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _repository.GetAccountByIdAsync(id);

            if (account == null)
            {
                return NotFound(new { message = "Account not found." });
            }

            bool result = await _repository.DeleteAccountAsync(id);

            if (result)
            {
                return Ok(new { message = "Account deleted successfully." });
            }

            return BadRequest(new { message = "Failed to delete account." });
        }
    }
}
