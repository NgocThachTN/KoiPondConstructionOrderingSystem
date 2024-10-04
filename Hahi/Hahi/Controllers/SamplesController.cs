using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hahi.ModelsV1;
using Hahi.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace Hahi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SamplesController : ControllerBase
    {
        private readonly ISamplesRepository _repository;

        public SamplesController(ISamplesRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Samples
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sample>>> GetSamples()
        {
            var samples = await _repository.GetSamplesAsync();
            return Ok(samples);
        }

        // GET: api/Samples/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sample>> GetSample(int id)
        {
            var sample = await _repository.GetSampleByIdAsync(id);

            if (sample == null)
            {
                return NotFound();
            }

            return Ok(sample);
        }

        // PUT: api/Samples/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSample(int id, Sample sample)
        {
            if (id != sample.SampleId)
            {
                return BadRequest();
            }

            try
            {
                await _repository.UpdateSampleAsync(sample);
            }
            catch
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

            return NoContent();
        }

        // POST: api/Samples
        [HttpPost]
        public async Task<ActionResult<Sample>> PostSample(Sample sample)
        {
            await _repository.AddSampleAsync(sample);
            return CreatedAtAction(nameof(GetSample), new { id = sample.SampleId }, sample);
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

            await _repository.DeleteSampleAsync(id);
            return NoContent();
        }
    }
}
