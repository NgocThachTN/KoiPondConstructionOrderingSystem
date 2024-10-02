using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hahi.Models;
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
        private readonly KoiContext _context;

        public AccountsController(IAccountsRepository repository, KoiContext context)
        {
            _context = context;
            _repository = repository;
        }

        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            var accounts = await _repository.GetAccounts().ToListAsync();
            return Ok(accounts);
        }
        [HttpGet("{AccountId}")]
        public IActionResult GetAccountById([FromRoute] int AccountId)
        {
            var account = _context.Accounts.FindAsync(AccountId);
            if (account == null)
            {
                return BadRequest();
            }
            return Ok(account);
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var account = await _repository.GetAccountByIdAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account.ToAccountDto());
        }

        // PUT: api/Accounts/5
        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateAccount([FromRoute] int id, [FromBody] UpdateAccountRequestDto modelUpdate)
        {
            var accountModel = _context.Accounts.FirstOrDefault(x => x.AccountId == id);
            if (accountModel == null)
            {
                return NotFound();
            }
            accountModel.UserName = modelUpdate.UserName;
            accountModel.Email = modelUpdate.Email;
            accountModel.Password = modelUpdate.Password;
            _context.SaveChanges();

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

