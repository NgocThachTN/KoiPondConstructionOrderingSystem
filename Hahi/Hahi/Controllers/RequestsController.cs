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
    public class RequestsController : ControllerBase
    {
        private readonly IRequestsRepository _repository;

        public RequestsController(IRequestsRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
            var requests = await _repository.GetRequestsAsync();
            return Ok(requests);
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _repository.GetRequestByIdAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            return Ok(request);
        }

        // PUT: api/Requests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (id != request.RequestId)
            {
                return BadRequest();
            }

            try
            {
                await _repository.UpdateRequestAsync(request);
            }
            catch
            {
                if (!await _repository.RequestExistsAsync(id))
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

        // POST: api/Requests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request)
        {
            await _repository.AddRequestAsync(request);
            return CreatedAtAction(nameof(GetRequest), new { id = request.RequestId }, request);
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _repository.GetRequestByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            try
            {
                await _repository.DeleteRequestAsync(id);
            }
            catch
            {
                return StatusCode(500, "Error deleting request. It might have related data in other tables.");
            }

            return NoContent();
        }
    }
}
