using System.Collections.Generic;
using System.Threading.Tasks;
using Hahi.Models;

namespace Hahi.Repositories
{
    public interface IMaintenanceRequestsRepository
    {
        Task<IEnumerable<MaintenanceRequest>> GetMaintenanceRequestsAsync();
        Task<MaintenanceRequest?> GetMaintenanceRequestByIdAsync(int maintenanceId, int requestId);
        Task AddMaintenanceRequestAsync(MaintenanceRequest maintenanceRequest);
        Task UpdateMaintenanceRequestAsync(MaintenanceRequest maintenanceRequest);
        Task DeleteMaintenanceRequestAsync(int maintenanceId, int requestId);
        Task<bool> MaintenanceRequestExistsAsync(int maintenanceId, int requestId);
    }
}
