using System.Collections.Generic;
using System.Threading.Tasks;
using Hahi.ModelsV1;

namespace Hahi.Repositories
{
    public interface ISamplesRepository
    {
        Task<IEnumerable<Sample>> GetSamplesAsync();
        Task<Sample?> GetSampleByIdAsync(int sampleId);
        Task AddSampleAsync(Sample sample);
        Task UpdateSampleAsync(Sample sample);
        Task<bool> DeleteSampleAsync(int sampleId);
        Task<bool> SampleExistsAsync(int sampleId);
    }
}
