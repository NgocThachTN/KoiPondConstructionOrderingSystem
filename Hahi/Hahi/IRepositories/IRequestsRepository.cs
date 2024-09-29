using Hahi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hahi.Repositories
{
    public interface IRequestsRepository
    {
        Task<IEnumerable<Request>> GetRequestsAsync();
        Task<Request> GetRequestByIdAsync(int id);
        Task AddRequestAsync(Request request);
        Task UpdateRequestAsync(Request request);
        Task DeleteRequestAsync(int id);
        Task<bool> RequestExistsAsync(int id);
    }
}
