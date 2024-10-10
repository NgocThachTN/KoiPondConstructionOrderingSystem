using KoiPond.Models;
using Microsoft.EntityFrameworkCore;
namespace KoiPond.Repositories
{
    public class ContractsRepository : IContractsRepository
    {
        private readonly KoiContext _context;

        public ContractsRepository(KoiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Contract>> GetContractsAsync()
        {
            return await _context.Contracts
                .Include(c => c.Request)
                .ThenInclude(r => r.User)
                .ThenInclude(u => u.Account)
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
                .ThenInclude(u => u.Account)
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
