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
    public class DesignsController : ControllerBase
    {
        private readonly IDesignsRepository _repository;
        private readonly KoisV1Context _context;

        public DesignsController(IDesignsRepository repository, KoisV1Context context)
        {
            _repository = repository;
            _context = context;
        }

        // GET: api/Designs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Design>>> GetDesigns()
        {
            var designs = await _repository.GetDesignsAsync();
            return Ok(designs);
        }

        // GET: api/Designs/5
        [HttpGet("{DesignId}")]
        public async Task<ActionResult<Design>> GetDesignById(int DesignId)
        {
            var design = await _repository.GetDesignByIdAsync(DesignId);

            if (design == null)
            {
                return NotFound(new { message = "Design not found." });
            }

            try
            {
                return Ok(design.ToDesignDto());
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message }); 
            }
        }

        // PUT: api/Designs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDesign([FromRoute] int id, [FromBody] UpdateDesignRequestDto designUpdate)
        {
            // Fetch the design by ID
            var design = await _repository.GetDesignByIdAsync(id);

            if (design == null)
            {
                return NotFound(new { message = "Design not found." });
            }

            // Map the incoming DTO to the existing design entity
            design.DesignName = designUpdate.DesignName ?? design.DesignName;
            design.DesignSize = designUpdate.DesignSize ?? design.DesignSize;
            design.DesignPrice = designUpdate.DesignPrice ?? design.DesignPrice;
            design.DesignImage = designUpdate.DesignImage ?? design.DesignImage;

            // Handle ConstructionTypes (assuming a list, needs handling accordingly)
            if (designUpdate.ConstructionTypes != null && designUpdate.ConstructionTypes.Count > 0)
            {
                // Assuming that we're updating the first ConstructionType or implementing a similar logic
                design.ConstructionType = new ConstructionType
                {
                    ConstructionTypeName = designUpdate.ConstructionTypes.First().ConstructionTypeName
                };
            }

            try
            {
                // Update the design using the repository
                await _repository.UpdateDesignAsync(design);
                // Save changes to the database asynchronously
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
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

            // Return the updated design as a DTO
            return Ok(design.ToDesignDto());
        }



        // POST: api/Designs
        [HttpPost]
        public IActionResult CreateDesign([FromBody] CreateDesignRequestDto design)
        {
            var designModel = design.ToDesignFromCreatedDto();
            _context.Designs.Add(designModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetDesignById), new { DesignID = designModel.DesignId }, designModel.ToDesignDto());
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

            if (design.ConstructionType == null)
            {
                return NotFound(new { message = "ConstructionType not found." });
            }

            // Proceed to delete 
            bool result = await _repository.DeleteDesignAsync(id);

            if (result)
            {
                return Ok(new { message = "Design deleted successfully." });
            }

            return BadRequest(new { message = "Failed to delete Design." });
        }
    }
}
