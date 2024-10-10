

using KoiPond.Models;

namespace KoiPond.Repositories
{
    public interface IConstructionTypesRepository
    {
        Task<IEnumerable<ConstructionType>> GetConstructionTypesAsync();
        Task<ConstructionType?> GetConstructionTypeByIdAsync(int id);
        Task AddConstructionTypeAsync(ConstructionType constructionType);
        Task UpdateConstructionTypeAsync(ConstructionType constructionType);
        Task<bool> DeleteConstructionTypeAsync(int id);
        Task<bool> ConstructionTypeExistsAsync(int id);
    }
}
