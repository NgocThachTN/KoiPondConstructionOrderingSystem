
using KoiPond.Models;

namespace KoiPond.Repositories
{
    public interface IRequestsRepository
    {
        Task<IEnumerable<Request>> GetRequestsAsync();
        Task<Request> GetRequestByIdAsync(int id);
        Task AddRequestAsync(Request request);
        Task UpdateRequestAsync(Request request);
        Task<bool> DeleteRequestAsync(int id);
        Task<bool> RequestExistsAsync(int id);
    }
}
