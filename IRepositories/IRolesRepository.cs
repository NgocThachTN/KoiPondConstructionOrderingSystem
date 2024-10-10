using System.Collections.Generic;
using System.Threading.Tasks;
using KoiPond.Models;

namespace KoiPond.IRepositories

{
    public interface IRolesRepository
    {
        Task<IEnumerable<Role>> GetRolesAsync();
        Task<Role?> GetRoleByIdAsync(int id);
        Task AddRoleAsync(Role role);
        Task UpdateRoleAsync(Role role);
        Task DeleteRoleAsync(int id);
        Task<bool> RoleExistsAsync(int id);
    }
}
