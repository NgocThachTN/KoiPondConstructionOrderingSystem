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
    public class MaintenanceRequestsController : ControllerBase
    {
        private readonly IMaintenanceRequestsRepository _repository;

        public MaintenanceRequestsController(IMaintenanceRequestsRepository repository)
        {
            _repository = repository;
        }

        // GET: api/MaintenanceRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaintenanceRequest>>> GetMaintenanceRequests()
        {
            var maintenanceRequests = await _repository.GetMaintenanceRequestsAsync();
            return Ok(maintenanceRequests);
        }

        // GET: api/MaintenanceRequests/5/5
        [HttpGet("{maintenanceId}/{requestId}")]
        public async Task<ActionResult<MaintenanceRequest>> GetMaintenanceRequest(int maintenanceId, int requestId)
        {
            var maintenanceRequest = await _repository.GetMaintenanceRequestByIdAsync(maintenanceId, requestId);

            if (maintenanceRequest == null)
            {
                return NotFound();
            }

            return Ok(maintenanceRequest);
        }

        // PUT: api/MaintenanceRequests/5/5
        [HttpPut("{maintenanceId}/{requestId}")]
        public async Task<IActionResult> PutMaintenanceRequest(int maintenanceId, int requestId, MaintenanceRequest maintenanceRequest)
        {
            if (maintenanceId != maintenanceRequest.MaintenanceId || requestId != maintenanceRequest.RequestId)
            {
                return BadRequest();
            }

            try
            {
                await _repository.UpdateMaintenanceRequestAsync(maintenanceRequest);
            }
            catch
            {
                if (!await _repository.MaintenanceRequestExistsAsync(maintenanceId, requestId))
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

        // POST: api/MaintenanceRequests
        [HttpPost]
        public async Task<ActionResult<MaintenanceRequest>> PostMaintenanceRequest(MaintenanceRequest maintenanceRequest)
        {
            await _repository.AddMaintenanceRequestAsync(maintenanceRequest);
            return CreatedAtAction(nameof(GetMaintenanceRequest), new { maintenanceId = maintenanceRequest.MaintenanceId, requestId = maintenanceRequest.RequestId }, maintenanceRequest);
        }

        // DELETE: api/MaintenanceRequests/5/5
        [HttpDelete("{maintenanceId}/{requestId}")]
        public async Task<IActionResult> DeleteMaintenanceRequest(int maintenanceId, int requestId)
        {
            var maintenanceRequest = await _repository.GetMaintenanceRequestByIdAsync(maintenanceId, requestId);
            if (maintenanceRequest == null)
            {
                return NotFound();
            }

            await _repository.DeleteMaintenanceRequestAsync(maintenanceId, requestId);
            return NoContent();
        }
    }
}
