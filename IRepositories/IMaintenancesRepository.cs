﻿

using KoiPond.Models;

namespace KoiPond.Repositories
{
    public interface IMaintenancesRepository
    {
        Task<IEnumerable<Maintenance>> GetMaintenancesAsync();
        Task<Maintenance?> GetMaintenanceByIdAsync(int id);
        Task AddMaintenanceAsync(Maintenance maintenance);
        Task UpdateMaintenanceAsync(Maintenance maintenance);
        Task DeleteMaintenanceAsync(int id);
        Task<bool> MaintenanceExistsAsync(int id);
    }
}
