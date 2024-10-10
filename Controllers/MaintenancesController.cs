
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KoiPond.Repositories;
using KoiPond.Models;

namespace KoiPond.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MaintenancesController : ControllerBase
    {
        private readonly IMaintenancesRepository _repository;

        public MaintenancesController(IMaintenancesRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Maintenances
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Maintenance>>> GetMaintenances()
        {
            var maintenances = await _repository.GetMaintenancesAsync();
            return Ok(maintenances);
        }

        // GET: api/Maintenances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Maintenance>> GetMaintenance(int id)
        {
            var maintenance = await _repository.GetMaintenanceByIdAsync(id);

            if (maintenance == null)
            {
                return NotFound();
            }

            return Ok(maintenance);
        }

        // PUT: api/Maintenances/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaintenance(int id, Maintenance maintenance)
        {
            if (id != maintenance.MaintenanceId)
            {
                return BadRequest();
            }

            try
            {
                await _repository.UpdateMaintenanceAsync(maintenance);
            }
            catch
            {
                if (!await _repository.MaintenanceExistsAsync(id))
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

        // POST: api/Maintenances
        [HttpPost]
        public async Task<ActionResult<Maintenance>> PostMaintenance(Maintenance maintenance)
        {
            await _repository.AddMaintenanceAsync(maintenance);
            return CreatedAtAction(nameof(GetMaintenance), new { id = maintenance.MaintenanceId }, maintenance);
        }

        // DELETE: api/Maintenances/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaintenance(int id)
        {
            var maintenance = await _repository.GetMaintenanceByIdAsync(id);
            if (maintenance == null)
            {
                return NotFound();
            }

            await _repository.DeleteMaintenanceAsync(id);
            return NoContent();
        }
    }
}
