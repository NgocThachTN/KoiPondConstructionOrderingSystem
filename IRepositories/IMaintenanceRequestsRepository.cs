﻿
using KoiPond.Models;

namespace KoiPond.Repositories
{
    public interface IMaintenanceRequestsRepository
    {
        Task<IEnumerable<MaintenanceRequest>> GetMaintenanceRequestsAsync();
        Task<MaintenanceRequest?> GetMaintenanceRequestByIdAsync(int id);
        Task AddMaintenanceRequestAsync(MaintenanceRequest maintenanceRequest);
        Task UpdateMaintenanceRequestAsync(MaintenanceRequest maintenanceRequest);
        Task DeleteMaintenanceRequestAsync(int id);
        Task<bool> MaintenanceRequestExistsAsync(int id);
    }
}
