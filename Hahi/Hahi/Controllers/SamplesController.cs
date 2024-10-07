using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hahi.ModelsV1;
using Hahi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Hahi.AutoMapper;
using Hahi.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Hahi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SamplesController : ControllerBase
    {
        private readonly ISamplesRepository _repository;
        private readonly KoisV1Context _context;

        public SamplesController(ISamplesRepository repository, KoisV1Context context)
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
            // Fetch the design by ID
            var sample = await _repository.GetSampleByIdAsync(id);

            if (sample == null)
            {
                return NotFound(new { message = "Sample not found." });
            }

            // Map the incoming DTO to the existing design entity
            sample.SampleName = sampleUpdate.SampleName ?? sample.SampleName;
            sample.SampleSize = sampleUpdate.SampleSize ?? sample.SampleSize;
            sample.SamplePrice = sampleUpdate.SamplePrice ?? sample.SamplePrice;
            sample.SampleImage = sampleUpdate.SampleImage ?? sample.SampleImage;

            // Handle ConstructionTypes (assuming a list, needs handling accordingly)
            if (sampleUpdate.ConstructionTypes != null && sampleUpdate.ConstructionTypes.Count > 0)
            {
                // Assuming that we're updating the first ConstructionType or implementing a similar logic
                sample.ConstructionType = new ConstructionType
                {
                    ConstructionTypeName = sampleUpdate.ConstructionTypes.First().ConstructionTypeName
                };
            }

            try
            {
                // Update the design using the repository
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

            // Return the updated design as a DTO
            return Ok(sample.ToSampleDto());
        }

        // POST: api/Samples
        [HttpPost]
        public IActionResult CreateSample([FromBody] CreateSampleRequestDto sample)
        {
            var sampleModel = sample.ToSampleFromCreatedDto();
            _context.Samples.Add(sampleModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetSampleById), new { SampleID = sampleModel.SampleId }, sampleModel.ToSampleDto());
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
