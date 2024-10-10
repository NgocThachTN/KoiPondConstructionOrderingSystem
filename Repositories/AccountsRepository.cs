using Microsoft.EntityFrameworkCore;
using KoiPond.Models;

namespace KoiPond.Repositories
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
                .Include(a => a.User)
                    .ThenInclude(u => u.Requests)
                        .ThenInclude(r => r.Design)
                           .ThenInclude(d => d.ConstructionType)
                .Include(a => a.User)
                    .ThenInclude(u => u.Requests)
                        .ThenInclude(r => r.Sample)
                           .ThenInclude(s => s.ConstructionType)
                .Include(a => a.User)
                    .ThenInclude(u => u.Requests)
                        .ThenInclude(r => r.Contract)
                .Include(a => a.User)
                    .ThenInclude(u => u.Requests)
                        .ThenInclude(r => r.MaintenanceRequests)
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

        public async Task<bool> DeleteAccountAsync(int id)
        {
            // Tìm tài khoản theo ID
            var account = await GetAccountByIdAsync(id);

            // Nếu tài khoản không tồn tại, trả về false
            if (account == null)
            {
                return false;
            }

            // Tiến hành xóa tài khoản và lưu thay đổi
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
