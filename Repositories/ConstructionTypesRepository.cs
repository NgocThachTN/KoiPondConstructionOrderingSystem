
using KoiPond.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiPond.Repositories
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
                   .ThenInclude(d => d.Requests)
                .Include(ct => ct.Samples)
                   .ThenInclude(s => s.Requests)
                .ToListAsync();
        }

        public async Task<ConstructionType?> GetConstructionTypeByIdAsync(int id)
        {
            return await _context.ConstructionTypes
                .Include(ct => ct.Designs)
                   .ThenInclude(d => d.Requests)
                .Include(ct => ct.Samples)
                   .ThenInclude(s => s.Requests)
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

        public async Task<bool> DeleteConstructionTypeAsync(int id)
        {
            var constructionType = await GetConstructionTypeByIdAsync(id);
            // Nếu tài khoản không tồn tại, trả về false
            if (constructionType == null)
            {
                return false;
            }

            // Tiến hành xóa tài khoản và lưu thay đổi
            _context.ConstructionTypes.Remove(constructionType);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ConstructionTypeExistsAsync(int id)
        {
            return await _context.ConstructionTypes.AnyAsync(ct => ct.ConstructionTypeId == id);
        }
    }
}
