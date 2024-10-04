using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hahi.ModelsV1;

namespace Hahi.Repositories
{
    public class DesignsRepository : IDesignsRepository
    {
        private readonly KoisV1Context _context;

        public DesignsRepository(KoisV1Context context)
        {
            _context = context;
        }

        public async Task<List<Design>> GetDesignsAsync()
        {
            return await _context.Designs
                .Include(d => d.ConstructionType)
                .ToListAsync();
        }

        public async Task<Design?> GetDesignByIdAsync(int id)
        {
            return await _context.Designs
                .Include(d => d.ConstructionType)
                .FirstOrDefaultAsync(d => d.DesignId == id);
        }

        public async Task AddDesignAsync(Design design)
        {
            _context.Designs.Add(design);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDesignAsync(Design design)
        {
            _context.Entry(design).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteDesignAsync(int id)
        {
            var design = await GetDesignByIdAsync(id);
            if (design == null)
            {
                return false;
            }

            // Tiến hành xóa tài khoản và lưu thay đổi
            _context.Designs.Remove(design);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DesignExistsAsync(int id)
        {
            return await _context.Designs.AnyAsync(e => e.DesignId == id);
        }
    }
}
