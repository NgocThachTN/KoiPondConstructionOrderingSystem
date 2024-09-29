using System.Collections.Generic;
using System.Threading.Tasks;
using Hahi.Models;

namespace Hahi.Repositories
{
    public interface IDesignsRepository
    {
        Task<IEnumerable<Design>> GetDesignsAsync();
        Task<Design?> GetDesignByIdAsync(int id);
        Task AddDesignAsync(Design design);
        Task UpdateDesignAsync(Design design);
        Task DeleteDesignAsync(int id);
        Task<bool> DesignExistsAsync(int id);
    }
}
