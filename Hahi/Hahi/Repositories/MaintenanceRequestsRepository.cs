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
                .Include(mr => mr.MaintenanceRequestNavigation) // Use the correct navigation property
                .Include(mr => mr.Request)
                .ToListAsync();
        }

        public async Task<MaintenanceRequest?> GetMaintenanceRequestByIdAsync(int maintenanceId, int requestId)
        {
            return await _context.MaintenanceRequests
                .Include(mr => mr.MaintenanceRequestNavigation) // Use the correct navigation property
                .Include(mr => mr.Request)
                .FirstOrDefaultAsync(mr => mr.MaintenanceRequestId == maintenanceId && mr.RequestId == requestId);
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
                .AnyAsync(mr => mr.MaintenanceRequestId == maintenanceId && mr.RequestId == requestId);
        }
    }
}
