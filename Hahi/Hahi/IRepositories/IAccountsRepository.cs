using System.Collections.Generic;
using System.Threading.Tasks;
using Hahi.ModelsV1;

namespace Hahi.Repositories
{
    public interface IAccountsRepository
    {
        IQueryable<Account> GetAccounts();
        Task<Account?> GetAccountByIdAsync(int id);
        Task AddAccountAsync(Account account);
        Task UpdateAccountAsync(Account account);
        Task<bool> DeleteAccountAsync(int id);
        Task<bool> AccountExistsAsync(int id);
    }
}
