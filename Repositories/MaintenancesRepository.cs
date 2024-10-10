using KoiPond.Models;
using Microsoft.EntityFrameworkCore;


namespace KoiPond.Repositories
{
    public class MaintenancesRepository : IMaintenancesRepository
    {
        private readonly KoiContext _context;

        public MaintenancesRepository(KoiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Maintenance>> GetMaintenancesAsync()
        {
            return await _context.Maintenances
                .Include(m => m.MaintenanceRequests) // Include MaintenanceRequests relationship
                .ToListAsync();
        }

        public async Task<Maintenance?> GetMaintenanceByIdAsync(int id)
        {
            return await _context.Maintenances
                .Include(m => m.MaintenanceRequests) // Include MaintenanceRequests relationship
                .FirstOrDefaultAsync(m => m.MaintenanceId == id);
        }

        public async Task AddMaintenanceAsync(Maintenance maintenance)
        {
            _context.Maintenances.Add(maintenance);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMaintenanceAsync(Maintenance maintenance)
        {
            _context.Entry(maintenance).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMaintenanceAsync(int id)
        {
            var maintenance = await GetMaintenanceByIdAsync(id);
            if (maintenance != null)
            {
                _context.Maintenances.Remove(maintenance);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> MaintenanceExistsAsync(int id)
        {
            return await _context.Maintenances.AnyAsync(e => e.MaintenanceId == id);
        }
    }
}
