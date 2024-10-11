
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using KoiPond.Repositories;
using KoiPond.Models;
using KoiPond.AutoMapper;
using KoiPond.DTOs;

namespace KoiPond.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SamplesController : ControllerBase
    {
        private readonly ISamplesRepository _repository;
        private readonly KoiContext _context;

        public SamplesController(ISamplesRepository repository, KoiContext context)
        {
            _repository = repository;
            _context = context;
        }

        // GET: api/Samples
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sample>>> GetSamples()
        {
            var samples = await _repository.GetSamplesAsync();
            var requestDtos = samples.Select(sample => sample.ToSampleDto()).ToList(); // Using RequestMapper
            return Ok(requestDtos);
        }

        // GET: api/Samples/5
        [HttpGet("{SampleId}")]
        public async Task<ActionResult<Sample>> GetSampleById(int SampleId)
        {
            var sample = await _repository.GetSampleByIdAsync(SampleId);

            if (sample == null)
            {
                return NotFound(new { message = "Sample not found." });
            }

            try
            {
                return Ok(sample.ToSampleDto());
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // PUT: api/Samples/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSample([FromRoute] int id, [FromBody] UpdateSampleRequestDto sampleUpdate)
        {
            // Fetch the sample by ID
            var sample = await _repository.GetSampleByIdAsync(id);

            if (sample == null)
            {
                return NotFound(new { message = "Sample not found." });
            }

            // Map the incoming DTO to the existing sample entity
            sample.SampleName = sampleUpdate.SampleName ?? sample.SampleName;
            sample.SampleSize = sampleUpdate.SampleSize ?? sample.SampleSize;
            sample.SamplePrice = sampleUpdate.SamplePrice ?? sample.SamplePrice;
            sample.SampleImage = sampleUpdate.SampleImage ?? sample.SampleImage;

            // Handle ConstructionTypes
            if (sampleUpdate.ConstructionTypes != null && sampleUpdate.ConstructionTypes.Count > 0)
            {
                // Fetch existing ConstructionType if it already exists
                var existingConstructionType = await _context.ConstructionTypes
                    .FirstOrDefaultAsync(ct => ct.ConstructionTypeName == sampleUpdate.ConstructionTypes.First().ConstructionTypeName);

                if (existingConstructionType != null)
                {
                    // If it exists, assign the existing ConstructionType to the sample
                    sample.ConstructionType = existingConstructionType;
                }
                else
                {
                    // If not, create a new ConstructionType
                    sample.ConstructionType = new ConstructionType
                    {
                        ConstructionTypeName = sampleUpdate.ConstructionTypes.First().ConstructionTypeName
                    };
                }
            }

            try
            {
                // Update the sample using the repository
                await _repository.UpdateSampleAsync(sample);
                // Save changes to the database asynchronously
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _repository.SampleExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Return the updated sample as a DTO
            return Ok(sample.ToSampleDto());
        }


        // POST: api/Samples
        [HttpPost]
        public async Task<IActionResult> CreateDesign([FromBody] CreateSampleRequestDto sample)
        {
            // Check if a sample with the same ConstructionTypeName already exists
            var existingConstructionType = _context.ConstructionTypes
                .FirstOrDefault(ct => ct.ConstructionTypeName == sample.ConstructionTypes.First().ConstructionTypeName);
            var existingSample = await _context.Samples
                .FirstOrDefaultAsync(d => d.SampleName == sample.SampleName &&
                                          d.SampleSize == sample.SampleSize &&
                                          d.SamplePrice == sample.SamplePrice &&
                                          d.SampleImage == sample.SampleImage);

            if (existingConstructionType != null && existingSample != null)
            {
                // Optionally, you can return a conflict response if you don't want to create a new sample
                // return Conflict(new { message = "Sample with this ConstructionTypeName already exists." });

                // Alternatively, you can reuse the existing ConstructionType without creating a new one
                var sampleModel = sample.ToSampleFromCreatedDto();
                sampleModel.ConstructionType = existingConstructionType; // Link to existing ConstructionType

                _context.Samples.Add(sampleModel);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetSampleById), new { SampleID = sampleModel.SampleId }, sampleModel.ToSampleDto());
            }
            else
            {
                // If no existing ConstructionType, create new sample with new ConstructionType
                var sampleModel = sample.ToSampleFromCreatedDto();

                _context.Samples.Add(sampleModel);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetSampleById), new { SampleID = sampleModel.SampleId }, sampleModel.ToSampleDto());
            }
        }


        // DELETE: api/Samples/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSample(int id)
        {
            var sample = await _repository.GetSampleByIdAsync(id);
            if (sample == null)
            {
                return NotFound();
            }

            if (sample.ConstructionType == null)
            {
                return NotFound(new { message = "ConstructionType not found." });
            }

            // Proceed to delete 
            bool result = await _repository.DeleteSampleAsync((id));

            if (result)
            {
                return Ok(new { message = "Sample deleted successfully." });
            }

            return BadRequest(new { message = "Failed to delete Sample." });
        }
    }
}
