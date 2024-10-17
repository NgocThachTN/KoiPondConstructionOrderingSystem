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
            var requestDtos = designs.Select(design => design.ToDesignDto()).ToList(); // Using RequestMapper
            return Ok(requestDtos);
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

        // POST: api/Designs
        [HttpPost]
        public async Task<IActionResult> CreateDesign([FromBody] CreateDesignRequestDto designDto)
        {
            // Check if the ConstructionType already exists with the same name
            var existingConstructionType = await _context.ConstructionTypes
                .FirstOrDefaultAsync(ct => ct.ConstructionTypeName == designDto.ConstructionTypes.First().ConstructionTypeName);

            if (existingConstructionType != null)
            {
                // ConstructionType already exists, so return a conflict response
                return Conflict(new { message = $"ConstructionType '{existingConstructionType.ConstructionTypeName}' already exists with ID: {existingConstructionType.ConstructionTypeId}" });
            }

            // Check if the Design already exists with the same data
            var existingDesign = await _context.Designs
                .FirstOrDefaultAsync(d => d.DesignName == designDto.DesignName &&
                                          d.DesignSize == designDto.DesignSize &&
                                          d.DesignPrice == designDto.DesignPrice &&
                                          d.DesignImage == designDto.DesignImage);

            if (existingDesign != null)
            {
                // Design already exists, so return a conflict response
                return Conflict(new { message = $"Design '{existingDesign.DesignName}' already exists with ID: {existingDesign.DesignId}" });
            }

            // If both ConstructionType and Design do not exist, proceed to create them
            var constructionType = new ConstructionType
            {
                ConstructionTypeName = designDto.ConstructionTypes.First().ConstructionTypeName
            };

            _context.ConstructionTypes.Add(constructionType);
            await _context.SaveChangesAsync(); // Save to generate the new ConstructionTypeId

            // Create the new Design
            var designModel = designDto.ToDesignFromCreatedDto();
            designModel.ConstructionTypeId = constructionType.ConstructionTypeId; // Set the correct ConstructionTypeId
            _context.Designs.Add(designModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDesignById), new { DesignID = designModel.DesignId }, designModel.ToDesignDto());
        }


        // PUT: api/Designs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDesign([FromRoute] int id, [FromBody] UpdateDesignRequestDto designUpdate)
        {
            // Fetch the sample by ID
            var design = await _repository.GetDesignByIdAsync(id);

            if (design == null)
            {
                return NotFound(new { message = "Design not found." });
            }

            // Map the incoming DTO to the existing sample entity
            design.DesignName = designUpdate.DesignName ?? design.DesignName;
            design.DesignSize = designUpdate.DesignSize ?? design.DesignSize;
            design.DesignPrice = designUpdate.DesignPrice ?? design.DesignPrice;
            design.DesignImage = designUpdate.DesignImage ?? design.DesignImage;

            // Handle ConstructionTypes
            if (designUpdate.ConstructionTypes != null && designUpdate.ConstructionTypes.Count > 0)
            {
                // Fetch existing ConstructionType if it already exists
                var existingConstructionType = await _context.ConstructionTypes
                    .FirstOrDefaultAsync(ct => ct.ConstructionTypeName == designUpdate.ConstructionTypes.First().ConstructionTypeName);

                if (existingConstructionType != null)
                {
                    // If it exists, assign the existing ConstructionType to the sample
                    design.ConstructionType = existingConstructionType;
                }
                else
                {
                    // If not, create a new ConstructionType
                    design.ConstructionType = new ConstructionType
                    {
                        ConstructionTypeName = designUpdate.ConstructionTypes.First().ConstructionTypeName
                    };
                }
            }

            try
            {
                // Update the sample using the repository
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

            // Return the updated sample as a DTO
            return Ok(design.ToDesignDto());
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
