using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hahi.ModelsV1;
using Hahi.Repositories;

namespace Hahi.Repositories
{
    public class AccountsRepository : IAccountsRepository
    {
        private readonly KoisV1Context _context;

        public AccountsRepository(KoisV1Context context)
        {
            _context = context;
        }

        public IQueryable<Account> GetAccounts()
        {
            return _context.Accounts.Include(a => a.User); // Use Include to fetch related User data
        }

        public async Task<Account> GetAccountByIdAsync(int accountId)
        {
            return await _context.Accounts
                                 .Include(a => a.User) // Ensure User data is included
                                 .FirstOrDefaultAsync(a => a.AccountId == accountId);
        }



        public async Task AddAccountAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAccountAsync(Account account)
        {
            _context.Entry(account).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAccountAsync(int id)
        {
            // Find the account by ID
            var account = await GetAccountByIdAsync(id);

            // If the account does not exist, return false
            if (account == null)
            {
                return false;
            }

            // Find the related User and remove it before deleting the Account
            if (account.User != null)
            {
                _context.Users.Remove(account.User);
            }

            // Remove the account and save changes
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> AccountExistsAsync(int id)
        {
            return await _context.Accounts.AnyAsync(a => a.AccountId == id);
        }
    }
}
