using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hahi.ModelsV1;

namespace Hahi.Repositories
{
    public class SamplesRepository : ISamplesRepository
    {
        private readonly KoisV1Context _context;

        public SamplesRepository(KoisV1Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Sample>> GetSamplesAsync()
        {
            return await _context.Samples
                .Include(s => s.ConstructionType)   // Include ConstructionType liên quan
                .Include(s => s.Requests)           // Include Requests liên quan
                .ToListAsync();
        }

        public async Task<Sample?> GetSampleByIdAsync(int sampleId)
        {
            return await _context.Samples
                .Include(s => s.ConstructionType)   // Include ConstructionType liên quan
                .Include(s => s.Requests)           // Include Requests liên quan
                .FirstOrDefaultAsync(s => s.SampleId == sampleId);
        }

        public async Task AddSampleAsync(Sample sample)
        {
            _context.Samples.Add(sample);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSampleAsync(Sample sample)
        {
            _context.Entry(sample).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSampleAsync(int sampleId)
        {
            var sample = await GetSampleByIdAsync(sampleId);
            if (sample != null)
            {
                _context.Samples.Remove(sample);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> SampleExistsAsync(int sampleId)
        {
            return await _context.Samples.AnyAsync(s => s.SampleId == sampleId);
        }
    }
}
