using KoiPondConstructionManagement.Dtos.Account;
using KoiPondConstructionManagement.Mappers;
using KoiPondConstructionManagement.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoiPondConstructionManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : ControllerBase
    {
        private readonly PrototypeKoiv1Context _context;

        public AccountController(PrototypeKoiv1Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAccount()
        {
            var accounts = _context.Accounts.ToList();
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

        [HttpPost]
        [Authorize]
        public IActionResult CreateAccount([FromBody] CreateAccountRequestDto account)
        {
            var accountModel = account.ToAccountFromCreatedDto();
            _context.Accounts.Add(accountModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetAccountById), new { AccountID = accountModel.AccountId }, accountModel.ToAccountDto());
        }
        [HttpPut]
        [Route("{id}")]
        [Authorize]
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
        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public IActionResult DeleteAccount([FromRoute]int id)
        {
            var accountModel = _context.Accounts.FirstOrDefault(x => x.AccountId == id);
            if(accountModel == null){
                return NotFound();
            }
            _context.Accounts.Remove(accountModel);
            _context.SaveChanges();
            return NoContent();
        }
    }
}