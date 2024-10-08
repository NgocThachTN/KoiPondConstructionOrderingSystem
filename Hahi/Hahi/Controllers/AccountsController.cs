using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hahi.ModelsV1;
using Hahi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using Hahi.DTOs;
using Hahi.AutoMapper;

namespace Hahi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsRepository _repository;
        private readonly KoisV1Context _context;

        public AccountsController(IAccountsRepository repository, KoisV1Context context)
        {
            _context = context;
            _repository = repository;
        }

        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccounts()
        {
            var accounts = await _repository.GetAccounts()
                                            .Where(account => account.User != null)
                                            .ToListAsync();

            var requestDtos = accounts.Select(account => account.ToAccountDto())
                                      .Where(dto => dto != null)
                                      .ToList();

            return Ok(requestDtos);
        }



        // GET: api/Accounts/5
        [HttpGet("{AccountId}")]
        public async Task<ActionResult<Account>> GetAccountById(int AccountId)
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
        [Authorize(Roles = "Manager")]
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

    //[SwaggerRequestExample(typeof(Account), typeof(AccountExample))]
    /*        public class AccountExample : Swashbuckle.AspNetCore.Filters.IExamplesProvider<Account>
            {
                public Account GetExamples()
                {
                    return new Account
                    {
                        AccountId = 0,
                        UserName = "manager_user",
                        Email = "manager@example.com",
                        Password = "Password123",
                        User = new User
                        {
                            UserId = 0,
                            Name = "Manager Name",
                            PhoneNumber = "123456789",
                            Address = "123 Manager St",
                            RoleId = 1, 
                            Requests = new List<Request>
                    {
                        new Request
                        {
                            RequestId = 0,
                            RequestName = "Sample Request",
                            Description = "Sample Description",
                            SampleId = 0,
                            DesignId = 0,
                            Contract = new Contract
                            {
                                ContractId = 0,
                                ContractName = "Sample Contract",
                                StartDate = DateTime.UtcNow,
                                EndDate = DateTime.UtcNow.AddDays(30),
                                Status = "Active"
                            },
                            MaintenanceRequests = new List<MaintenanceRequest>
                            {
                                new MaintenanceRequest
                                {
                                    MaintenanceId = 0,
                                    Status = "In Progress",
                                    StartDate = DateTime.UtcNow,
                                    EndDate = DateTime.UtcNow.AddDays(7)
                                }
                            }
                        }
                    }
                        }
                    };
                }
            }*/
}

