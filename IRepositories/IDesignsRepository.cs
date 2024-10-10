

using KoiPond.Models;

namespace KoiPond.Repositories
{
    public interface IDesignsRepository
    {
        Task<List<Design>> GetDesignsAsync();
        Task<Design?> GetDesignByIdAsync(int id);
        Task AddDesignAsync(Design design);
        Task UpdateDesignAsync(Design design);
        Task<bool> DeleteDesignAsync(int id);
        Task<bool> DesignExistsAsync(int id);
    }
}
