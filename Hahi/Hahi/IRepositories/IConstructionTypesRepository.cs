using System.Collections.Generic;
using System.Threading.Tasks;
using Hahi.Models;

namespace Hahi.Repositories
{
    public interface IConstructionTypesRepository
    {
        Task<IEnumerable<ConstructionType>> GetConstructionTypesAsync();
        Task<ConstructionType?> GetConstructionTypeByIdAsync(int id);
        Task AddConstructionTypeAsync(ConstructionType constructionType);
        Task UpdateConstructionTypeAsync(ConstructionType constructionType);
        Task DeleteConstructionTypeAsync(int id);
        Task<bool> ConstructionTypeExistsAsync(int id);
    }
}
