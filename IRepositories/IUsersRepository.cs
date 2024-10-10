

using KoiPond.Models;

namespace KoiPond.Repositories
{
    public interface IUsersRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> UserExistsAsync(int id);
    }
}
