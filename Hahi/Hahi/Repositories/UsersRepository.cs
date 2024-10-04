using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hahi.ModelsV1;

namespace Hahi.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly KoisV1Context _context;

        public UsersRepository(KoisV1Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users
                                 .Include(u => u.Account) // Include related Account
                                 .Include(u => u.Role)    // Include related Role
                                 .ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users
                                 .Include(u => u.Account)
                                 .Include(u => u.Role)
                                 .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            // Tìm tài khoản theo ID
            var user = await GetUserByIdAsync(id);

            // Nếu tài khoản không tồn tại, trả về false
            if (user == null)
            {
                return false;
            }

            // Tiến hành xóa tài khoản và lưu thay đổi
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UserExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.UserId == id);
        }
    }
}
