using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hahi.Models;

namespace Hahi.Repositories
{
    public class ConstructionTypesRepository : IConstructionTypesRepository
    {
        private readonly KoiContext _context;

        public ConstructionTypesRepository(KoiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ConstructionType>> GetConstructionTypesAsync()
        {
            return await _context.ConstructionTypes
                .Include(ct => ct.Designs)
                .Include(ct => ct.Samples)
                .ToListAsync();
        }

        public async Task<ConstructionType?> GetConstructionTypeByIdAsync(int id)
        {
            return await _context.ConstructionTypes
                .Include(ct => ct.Designs)
                .Include(ct => ct.Samples)
                .FirstOrDefaultAsync(ct => ct.ConstructionTypeId == id);
        }

        public async Task AddConstructionTypeAsync(ConstructionType constructionType)
        {
            _context.ConstructionTypes.Add(constructionType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateConstructionTypeAsync(ConstructionType constructionType)
        {
            _context.Entry(constructionType).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteConstructionTypeAsync(int id)
        {
            var constructionType = await GetConstructionTypeByIdAsync(id);
            if (constructionType != null)
            {
                _context.ConstructionTypes.Remove(constructionType);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ConstructionTypeExistsAsync(int id)
        {
            return await _context.ConstructionTypes.AnyAsync(ct => ct.ConstructionTypeId == id);
        }
    }
}
