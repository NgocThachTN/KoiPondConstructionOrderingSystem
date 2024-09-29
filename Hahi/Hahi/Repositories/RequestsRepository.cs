using Hahi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hahi.Repositories
{
    public class RequestsRepository : IRequestsRepository
    {
        private readonly KoiContext _context;

        public RequestsRepository(KoiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Request>> GetRequestsAsync()
        {
            return await _context.Requests
                                 .Include(r => r.User)
                                 .Include(r => r.Sample)
                                 .Include(r => r.Design)
                                 .Include(r => r.Contract)
                                 .ToListAsync();
        }

        public async Task<Request> GetRequestByIdAsync(int id)
        {
            return await _context.Requests
                                 .Include(r => r.User)
                                 .Include(r => r.Sample)
                                 .Include(r => r.Design)
                                 .Include(r => r.Contract)
                                 .FirstOrDefaultAsync(r => r.RequestId == id);
        }

        public async Task AddRequestAsync(Request request)
        {
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRequestAsync(Request request)
        {
            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRequestAsync(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request != null)
            {
                _context.Requests.Remove(request);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> RequestExistsAsync(int id)
        {
            return await _context.Requests.AnyAsync(e => e.RequestId == id);
        }
    }
}
