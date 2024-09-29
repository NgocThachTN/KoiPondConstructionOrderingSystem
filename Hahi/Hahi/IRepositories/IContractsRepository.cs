using System.Collections.Generic;
using System.Threading.Tasks;
using Hahi.Models;

namespace Hahi.Repositories
{
    public interface IContractsRepository
    {
        Task<IEnumerable<Contract>> GetContractsAsync();
        Task<Contract?> GetContractByIdAsync(int id);
        Task AddContractAsync(Contract contract);
        Task UpdateContractAsync(Contract contract);
        Task DeleteContractAsync(int id);
        Task<bool> ContractExistsAsync(int id);
    }
}
