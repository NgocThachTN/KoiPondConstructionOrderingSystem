
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
    public class ConstructionTypesController : ControllerBase
    {
        private readonly IConstructionTypesRepository _repository;
        private readonly KoiContext _context;

        public ConstructionTypesController(IConstructionTypesRepository repository, KoiContext context)
        {
            _repository = repository;
            _context = context;
        }

        // GET: api/ConstructionTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConstructionType>>> GetConstructionTypes()
        {
            var constructionTypes = await _repository.GetConstructionTypesAsync();
            var requestDtos = constructionTypes.Select(constructionType => constructionType.ToConstructionTypeDto()).ToList(); // Using RequestMapper
            return Ok(requestDtos);
        }

        // GET: api/ConstructionTypes/5
        [HttpGet("{ConstructionTypeId}")]
        public async Task<ActionResult<ConstructionType>> GetConstructionTypeById(int ConstructionTypeId)
        {
            var constructionType = await _repository.GetConstructionTypeByIdAsync(ConstructionTypeId);

            if (constructionType == null)
            {
                return NotFound(new { message = "ConstructionType not found." });
            }

            try
            {
                return Ok(constructionType.ToConstructionTypeDto());
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message }); // Return NotFound if Account is null
            }
        }

        // PUT: api/ConstructionTypes/5
        [HttpPut("{id}")]
        public IActionResult UpdateConstructionType([FromRoute] int id, [FromBody] UpdateConstructionTypeRequestDto constructionTypeUpdate)
        {
            // Fetch ConstructionType including Designs and Samples
            var constructionTypeModel = _context.ConstructionTypes
                .Include(ct => ct.Designs)
                .Include(ct => ct.Samples)
                .FirstOrDefault(x => x.ConstructionTypeId == id);

            if (constructionTypeModel == null)
            {
                return NotFound(new { message = "ConstructionType not found." });
            }

            // Update the ConstructionType name
            constructionTypeModel.ConstructionTypeName = constructionTypeUpdate.ConstructionName;

            // Update Designs
            if (constructionTypeUpdate.Designs != null)
            {
                // Clear existing designs if required
                constructionTypeModel.Designs.Clear();

                foreach (var designDto in constructionTypeUpdate.Designs)
                {
                    constructionTypeModel.Designs.Add(new Design
                    {
                        DesignName = designDto.DesignName,
                        DesignSize = designDto.DesignSize,
                        DesignPrice = designDto.DesignPrice,
                        DesignImage = designDto.DesignImage
                    });
                }
            }

            // Update Samples
            if (constructionTypeUpdate.Samples != null)
            {
                // Clear existing samples if required
                constructionTypeModel.Samples.Clear();

                foreach (var sampleDto in constructionTypeUpdate.Samples)
                {
                    constructionTypeModel.Samples.Add(new Sample
                    {
                        SampleName = sampleDto.SampleName,
                        SampleSize = sampleDto.SampleSize,
                        SamplePrice = sampleDto.SamplePrice,
                        SampleImage = sampleDto.SampleImage
                    });
                }
            }

            // Save changes to the database
            _context.SaveChanges();

            // Return the updated ConstructionType as a DTO
            return Ok(constructionTypeModel.ToConstructionTypeDto());
        }


        // POST: api/ConstructionTypes
        [HttpPost]
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public IActionResult CreateConstructionType([FromBody] CreateConstructionTypeRequestDto constructionType)
        {
            var constructionTypeModel = constructionType.ToConstructionTypeFromCreatedDto();
            _context.ConstructionTypes.Add(constructionTypeModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetConstructionTypeById), new { ConstructionTypeID = constructionTypeModel.ConstructionTypeId }, constructionTypeModel.ToConstructionTypeDto());
        }

        // DELETE: api/ConstructionTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConstructionType(int id)
        {
            var constructionType = await _repository.GetConstructionTypeByIdAsync(id);
            if (constructionType == null)
            {
                return NotFound();
            }

            // Check if the associated Account exists
            if (constructionType.Designs == null)
            {
                return NotFound(new { message = "Design not found." });
            }

            if (constructionType.Samples == null)
            {
                return NotFound(new { message = "Sample not found." });
            }

            // Proceed to delete 
            bool result = await _repository.DeleteConstructionTypeAsync(id);

            if (result)
            {
                return Ok(new { message = "ConstructionType deleted successfully." });
            }

            return BadRequest(new { message = "Failed to delete ConstructionType." });
        }

    }
}
