using System.Numerics;
using KoiPond.Models;
using Microsoft.EntityFrameworkCore;


namespace KoiPond.Repositories
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
                                 .ThenInclude(u => u.Account)
                                 .Include(r => r.Sample)
                                 .Include(r => r.Design)
                                 .Include(r => r.Contract)
                                 .ToListAsync();
        }

        public async Task<Request> GetRequestByIdAsync(int id)
        {
            return await _context.Requests
                                 .Include(r => r.User)
                                 .ThenInclude(u => u.Account)
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

        public async Task<bool> UpdateRequestAsync(Request request)
        {
                        try
            {
                _context.Requests.Update(request);
                await _context.SaveChangesAsync();
                return true; // Return true to indicate success
            }
            catch
            {
                return false; // Return false to indicate failure
            }
        }

        public async Task<bool> DeleteRequestAsync(int id)
        {
            var request = await _context.Requests.Include(r => r.Design)
                                                 .Include(r => r.Sample)
                                                 .FirstOrDefaultAsync(r => r.RequestId == id);
            if (request != null)
            {
                // Kiểm tra và xóa Design nếu có
                if (request.Design != null)
                {
                    _context.Designs.Remove(request.Design);
                }

                // Kiểm tra và xóa Sample nếu có
                if (request.Sample != null)
                {
                    _context.Samples.Remove(request.Sample);
                }

                // Xóa request
                _context.Requests.Remove(request);
                await _context.SaveChangesAsync();
                return true; // Xóa thành công
            }
            return false; // Không tìm thấy request
        }


        public async Task<bool> RequestExistsAsync(int id)
        {
            return await _context.Requests.AnyAsync(e => e.RequestId == id);
        }
    }
}
