using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hahi.Models;
using Hahi.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace Hahi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConstructionTypesController : ControllerBase
    {
        private readonly IConstructionTypesRepository _repository;

        public ConstructionTypesController(IConstructionTypesRepository repository)
        {
            _repository = repository;
        }

        // GET: api/ConstructionTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConstructionType>>> GetConstructionTypes()
        {
            var constructionTypes = await _repository.GetConstructionTypesAsync();
            return Ok(constructionTypes);
        }

        // GET: api/ConstructionTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConstructionType>> GetConstructionType(int id)
        {
            var constructionType = await _repository.GetConstructionTypeByIdAsync(id);

            if (constructionType == null)
            {
                return NotFound();
            }

            return Ok(constructionType);
        }

        // PUT: api/ConstructionTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConstructionType(int id, ConstructionType constructionType)
        {
            if (id != constructionType.ConstructionTypeId)
            {
                return BadRequest();
            }

            try
            {
                await _repository.UpdateConstructionTypeAsync(constructionType);
            }
            catch
            {
                if (!await _repository.ConstructionTypeExistsAsync(id))
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

        // POST: api/ConstructionTypes
        [HttpPost]
        public async Task<ActionResult<ConstructionType>> PostConstructionType(ConstructionType constructionType)
        {
            await _repository.AddConstructionTypeAsync(constructionType);
            return CreatedAtAction(nameof(GetConstructionType), new { id = constructionType.ConstructionTypeId }, constructionType);
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

            await _repository.DeleteConstructionTypeAsync(id);
            return NoContent();
        }
    }
}
