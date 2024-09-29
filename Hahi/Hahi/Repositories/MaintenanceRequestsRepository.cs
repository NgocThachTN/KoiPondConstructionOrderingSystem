using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hahi.Models;

namespace Hahi.Repositories
{
    public class MaintenanceRequestsRepository : IMaintenanceRequestsRepository
    {
        private readonly KoiContext _context;

        public MaintenanceRequestsRepository(KoiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MaintenanceRequest>> GetMaintenanceRequestsAsync()
        {
            return await _context.MaintenanceRequests
                .Include(mr => mr.Maintenance)
                .Include(mr => mr.Request)
                .ToListAsync();
        }

        public async Task<MaintenanceRequest?> GetMaintenanceRequestByIdAsync(int maintenanceId, int requestId)
        {
            return await _context.MaintenanceRequests
                .Include(mr => mr.Maintenance)
                .Include(mr => mr.Request)
                .FirstOrDefaultAsync(mr => mr.MaintenanceId == maintenanceId && mr.RequestId == requestId);
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

        public async Task DeleteMaintenanceRequestAsync(int maintenanceId, int requestId)
        {
            var maintenanceRequest = await GetMaintenanceRequestByIdAsync(maintenanceId, requestId);
            if (maintenanceRequest != null)
            {
                _context.MaintenanceRequests.Remove(maintenanceRequest);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> MaintenanceRequestExistsAsync(int maintenanceId, int requestId)
        {
            return await _context.MaintenanceRequests
                .AnyAsync(mr => mr.MaintenanceId == maintenanceId && mr.RequestId == requestId);
        }
    }
}
