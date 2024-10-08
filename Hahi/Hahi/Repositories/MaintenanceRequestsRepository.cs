using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hahi.ModelsV1;

namespace Hahi.Repositories
{
    public class MaintenanceRequestsRepository : IMaintenanceRequestsRepository
    {
        private readonly KoisV1Context _context;

        public MaintenanceRequestsRepository(KoisV1Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MaintenanceRequest>> GetMaintenanceRequestsAsync()
        {
            return await _context.MaintenanceRequests
                .Include(mr => mr.MaintenanceRequestNavigation) // Eagerly load related MaintenanceRequestNavigation data
                .Include(mr => mr.Request)
                    .ThenInclude(r => r.User)
                         .ThenInclude(u => u.Account)// Eagerly load related User data within the Request
                .Include(mr => mr.Request.Design) // Eagerly load Design data within the Request
                .Include(mr => mr.Request.Sample) // Eagerly load Sample data within the Request
                .ToListAsync();
        }

        public async Task<MaintenanceRequest?> GetMaintenanceRequestByIdAsync(int id)
        {
            return await _context.MaintenanceRequests
                .Include(mr => mr.MaintenanceRequestNavigation) // Eagerly load related MaintenanceRequestNavigation data
                .Include(mr => mr.Request)
                    .ThenInclude(r => r.User)
                        .ThenInclude(u => u.Account)// Eagerly load related User data within the Request
                .Include(mr => mr.Request.Design) // Eagerly load Design data within the Request
                .Include(mr => mr.Request.Sample) // Eagerly load Sample data within the Request
                .FirstOrDefaultAsync(mr => mr.MaintenanceRequestId == id);
        }



        public async Task AddMaintenanceRequestAsync(MaintenanceRequest maintenanceRequest)
        {
            _context.MaintenanceRequests.Add(maintenanceRequest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMaintenanceRequestAsync(MaintenanceRequest maintenanceRequest)
        {
            _context.Entry(maintenanceRequest).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMaintenanceRequestAsync(int id)
        {
            var maintenanceRequest = await GetMaintenanceRequestByIdAsync(id);
            if (maintenanceRequest != null)
            {
                _context.MaintenanceRequests.Remove(maintenanceRequest);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> MaintenanceRequestExistsAsync(int id)
        {
            return await _context.MaintenanceRequests
                .AnyAsync(mr => mr.MaintenanceRequestId == id);
        }
    }
}
