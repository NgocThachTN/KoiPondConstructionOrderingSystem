using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hahi.Models;
using Hahi.Repositories;

namespace Hahi.Repositories
{
    public class AccountsRepository : IAccountsRepository
    {
        private readonly KoiContext _context;

        public AccountsRepository(KoiContext context)
        {
            _context = context;
        }

        public IQueryable<Account> GetAccounts()
        {
            return _context.Accounts
                .Include(a => a.User)
                    .ThenInclude(u => u.Requests)
                        .ThenInclude(r => r.Design)
                           .ThenInclude(k => k.ConstructionType)
                .Include(a => a.User)
                    .ThenInclude(u => u.Requests)
                        .ThenInclude(r => r.Sample)
                          .ThenInclude(k => k.ConstructionType)
                .Include(a => a.User)
                    .ThenInclude(u => u.Requests)
                        .ThenInclude(r => r.Contract)
                .Include(a => a.User)
                    .ThenInclude(u => u.Requests)
                        .ThenInclude(r => r.MaintenanceRequests); 
        }

        public async Task<Account?> GetAccountByIdAsync(int id)
        {
            return await _context.Accounts
                                 .Include(a => a.User) // Include the related User entity
                                 .FirstOrDefaultAsync(a => a.AccountId == id);
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

        public async Task DeleteAccountAsync(int id)
        {
            var account = await GetAccountByIdAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> AccountExistsAsync(int id)
        {
            return await _context.Accounts.AnyAsync(a => a.AccountId == id);
        }
    }
}
