using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hahi.Models;
using Hahi.Repositories;

namespace Hahi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignsController : ControllerBase
    {
        private readonly IDesignsRepository _repository;

        public DesignsController(IDesignsRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Designs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Design>>> GetDesigns()
        {
            var designs = await _repository.GetDesignsAsync();
            return Ok(designs);
        }

        // GET: api/Designs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Design>> GetDesign(int id)
        {
            var design = await _repository.GetDesignByIdAsync(id);

            if (design == null)
            {
                return NotFound();
            }

            return Ok(design);
        }

        // PUT: api/Designs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDesign(int id, Design design)
        {
            if (id != design.DesignId)
            {
                return BadRequest();
            }

            try
            {
                await _repository.UpdateDesignAsync(design);
            }
            catch
            {
                if (!await _repository.DesignExistsAsync(id))
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

        // POST: api/Designs
        [HttpPost]
        public async Task<ActionResult<Design>> PostDesign(Design design)
        {
            await _repository.AddDesignAsync(design);
            return CreatedAtAction(nameof(GetDesign), new { id = design.DesignId }, design);
        }

        // DELETE: api/Designs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDesign(int id)
        {
            var design = await _repository.GetDesignByIdAsync(id);
            if (design == null)
            {
                return NotFound();
            }

            await _repository.DeleteDesignAsync(id);
            return NoContent();
        }
    }
}
