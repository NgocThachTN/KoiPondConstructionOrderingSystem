using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hahi.ModelsV1;

namespace Hahi.Repositories
{
    public class ContractsRepository : IContractsRepository
    {
        private readonly KoisV1Context _context;

        public ContractsRepository(KoisV1Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Contract>> GetContractsAsync()
        {
            return await _context.Contracts
                .Include(c => c.Request)
                .ThenInclude(r => r.User)
                .Include(c => c.Request)
                .ThenInclude(r => r.Design)
                .Include(c => c.Request)
                .ThenInclude(r => r.Sample)
                .ToListAsync();
        }

        public async Task<Contract> GetContractByIdAsync(int id)
        {
            return await _context.Contracts
                .Include(c => c.Request)
                .ThenInclude(r => r.User)
                .Include(c => c.Request)
                .ThenInclude(r => r.Design)
                .Include(c => c.Request)
                .ThenInclude(r => r.Sample)
                .FirstOrDefaultAsync(c => c.ContractId == id);
        }


        public async Task AddContractAsync(Contract contract)
        {
            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateContractAsync(Contract contract)
        {
            _context.Entry(contract).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteContractAsync(int id)
        {
            var contract = await GetContractByIdAsync(id);
            if (contract != null)
            {
                _context.Contracts.Remove(contract);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ContractExistsAsync(int id)
        {
            return await _context.Contracts.AnyAsync(e => e.ContractId == id);
        }
    }
}
