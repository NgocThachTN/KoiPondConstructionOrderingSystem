using Microsoft.EntityFrameworkCore;
using KoiPond.Models;
using KoiPond.IRepositories;

namespace KoiPond.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly KoiContext _context;

        public RolesRepository(KoiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetRolesAsync()
        {
            return await _context.Roles
                .Include(r => r.Users) // Include Users relationship
                .ToListAsync();
        }

        public async Task<Role?> GetRoleByIdAsync(int id)
        {
            return await _context.Roles
                .Include(r => r.Users) // Include Users relationship
                .FirstOrDefaultAsync(r => r.RoleId == id);
        }

        public async Task AddRoleAsync(Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoleAsync(Role role)
        {
            _context.Entry(role).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoleAsync(int id)
        {
            var role = await GetRoleByIdAsync(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> RoleExistsAsync(int id)
        {
            return await _context.Roles.AnyAsync(r => r.RoleId == id);
        }
    }
}
