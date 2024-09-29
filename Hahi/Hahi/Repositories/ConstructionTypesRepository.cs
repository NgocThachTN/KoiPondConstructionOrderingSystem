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
            return await _context.ConstructionTypes.ToListAsync();
        }

        public async Task<ConstructionType?> GetConstructionTypeByIdAsync(int id)
        {
            return await _context.ConstructionTypes.FindAsync(id);
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
