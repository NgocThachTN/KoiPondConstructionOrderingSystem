
using Microsoft.EntityFrameworkCore;
using KoiPond.Models;

namespace KoiPond.Repositories
{
    public class SamplesRepository : ISamplesRepository
    {
        private readonly KoiContext _context;

        public SamplesRepository(KoiContext context)
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

        public async Task<bool> DeleteSampleAsync(int sampleId)
        {
            var sample = await GetSampleByIdAsync(sampleId);
            if (sample == null)
            {
                _context.Samples.Remove(sample);
                return false;
            }

            // Tiến hành xóa tài khoản và lưu thay đổi
            _context.Samples.Remove(sample);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SampleExistsAsync(int sampleId)
        {
            return await _context.Samples.AnyAsync(s => s.SampleId == sampleId);
        }
    }
}
