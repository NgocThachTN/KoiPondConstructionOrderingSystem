using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hahi.ModelsV1;
using Hahi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Hahi.DTOs; // Ensure you include this to access the DTOs
using Hahi.AutoMapper; // Include for the RequestMapper and ContractMapper

namespace Hahi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestsRepository _repository;
        private KoisV1Context _context;

        public RequestsController(IRequestsRepository repository, KoisV1Context context)
        {
            _repository = repository;
            _context = context;
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestDto>>> GetRequests()
        {
            var requests = await _repository.GetRequestsAsync();
            var requestDtos = requests.Select(request => request.ToRequestDto()).ToList(); // Using RequestMapper
            return Ok(requestDtos);
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestDto>> GetRequest(int id)
        {
            var request = await _repository.GetRequestByIdAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            var requestDto = request.ToRequestDto(); // Using RequestMapper
            return Ok(requestDto);
        }


        // POST: api/Requests/ByDesign
        [HttpPost("ByDesign")]
        public async Task<ActionResult<RequestDto>> PostRequestByDesign(CreateRequestDesignDto requestDto)
        {
            if (!requestDto.IsDesignSelected)
            {
                return BadRequest("Design must be selected for this endpoint.");
            }

            var (request, errorMessage) = await requestDto.ToRequestDesignFromCreatedDto(_context);

            if (request == null)
            {
                return BadRequest(errorMessage); // Return the specific error message if validation fails
            }

            await _repository.AddRequestAsync(request);
            var requestResultDto = request.ToRequestDto(); // Using RequestMapper
            return CreatedAtAction(nameof(GetRequest), new { id = request.RequestId }, requestResultDto);
        }


        // POST: api/Requests/BySample
        [HttpPost("BySample")]
        public async Task<ActionResult<RequestDto>> PostRequestBySample(CreateRequestSampleDto requestDto)
        {
            if (!requestDto.IsSampleSelected)
            {
                return BadRequest("Sample must be selected for this endpoint.");
            }

            var (request, errorMessage) = await requestDto.ToRequestSampleFromCreatedDtoAsync(_context);

            if (request == null)
            {
                return BadRequest(errorMessage); // Return the specific error message if validation fails
            }

            await _repository.AddRequestAsync(request);
            var requestResultDto = request.ToRequestDto(); // Using RequestMapper
            return CreatedAtAction(nameof(GetRequest), new { id = request.RequestId }, requestResultDto);
        }

        // PUT: api/Requests/ByDesign/5
        [HttpPut("ByDesign/{id}")]
        public async Task<ActionResult<RequestDto>> PutRequestByDesign(int id, UpdateRequestDesignDto requestDto)
        {
            var existingRequest = await _repository.GetRequestByIdAsync(id);
            if (existingRequest == null)
            {
                return NotFound("Request not found.");
            }

            // Update the Request details
            existingRequest.RequestName = requestDto.RequestName;
            existingRequest.Description = requestDto.Description;

            // Update the User and Account information without creating a new User
            if (existingRequest.User != null)
            {
                existingRequest.User.Name = requestDto.User.Name;
                existingRequest.User.PhoneNumber = requestDto.User.PhoneNumber;
                existingRequest.User.Address = requestDto.User.Address;

                if (existingRequest.User.Account != null)
                {
                    existingRequest.User.Account.UserName = requestDto.User.UserName;
                    existingRequest.User.Account.Email = requestDto.User.Email;
                    existingRequest.User.Account.Password = requestDto.User.Password;
                }

                existingRequest.User.RoleId = requestDto.User.RoleId;
            }

            // Update the Design information without creating a new Design
            if (existingRequest.Design != null && requestDto.Design != null)
            {
                existingRequest.Design.DesignName = requestDto.Design.DesignName;
                existingRequest.Design.DesignSize = requestDto.Design.DesignSize;
                existingRequest.Design.DesignPrice = requestDto.Design.DesignPrice;
                existingRequest.Design.DesignImage = requestDto.Design.DesignImage;
            }

            await _repository.UpdateRequestAsync(existingRequest);
            var requestResultDto = existingRequest.ToRequestDto(); // Using RequestMapper
            return Ok(requestResultDto);
        }

        // PUT: api/Requests/BySample/5
        [HttpPut("BySample/{id}")]
        public async Task<ActionResult<RequestDto>> PutRequestBySample(int id, UpdateRequestSampleDto requestDto)
        {
            var existingRequest = await _repository.GetRequestByIdAsync(id);
            if (existingRequest == null)
            {
                return NotFound("Request not found.");
            }

            // Update Request details
            existingRequest.RequestName = requestDto.RequestName;
            existingRequest.Description = requestDto.Description;

            // Update the User and Account information without creating a new User
            if (existingRequest.User != null)
            {
                existingRequest.User.Name = requestDto.User.Name;
                existingRequest.User.PhoneNumber = requestDto.User.PhoneNumber;
                existingRequest.User.Address = requestDto.User.Address;

                if (existingRequest.User.Account != null)
                {
                    existingRequest.User.Account.UserName = requestDto.User.UserName;
                    existingRequest.User.Account.Email = requestDto.User.Email;
                    existingRequest.User.Account.Password = requestDto.User.Password;
                }

                existingRequest.User.RoleId = requestDto.User.RoleId;
            }

            // Update Sample information without creating a new Sample
            if (existingRequest.Sample != null && requestDto.Sample != null)
            {
                existingRequest.Sample.SampleName = requestDto.Sample.SampleName;
                existingRequest.Sample.SampleSize = requestDto.Sample.SampleSize;
                existingRequest.Sample.SamplePrice = requestDto.Sample.SamplePrice;
                existingRequest.Sample.SampleImage = requestDto.Sample.SampleImage;
            }

            // Update the request in the database
            await _repository.UpdateRequestAsync(existingRequest);
            var requestResultDto = existingRequest.ToRequestDto(); // Using RequestMapper

            return Ok(requestResultDto);
        }


        // DELETE: api/Requests/ByDesign/5
        [HttpDelete("ByDesign/{id}")]
        public async Task<ActionResult<RequestDto>> DeleteRequestByDesign(int id)
        {
            var existingRequest = await _repository.GetRequestByIdAsync(id);
            if (existingRequest == null)
            {
                return NotFound("Request not found.");
            }

            if (existingRequest.Design == null)
            {
                return BadRequest("This request does not have a design associated with it.");
            }

            // Disassociate the Design before deleting the Request
            existingRequest.DesignId = null; // Set the foreign key to null
            existingRequest.Design = null; // Set the navigation property to null

            // Save the changes to update the database
            var updateResult = await _repository.UpdateRequestAsync(existingRequest);
            if (!updateResult)
            {
                return StatusCode(500, "Error updating the request to disassociate the design.");
            }

            // Proceed with the deletion
            var deleteResult = await _repository.DeleteRequestAsync(id);
            if (!deleteResult)
            {
                return StatusCode(500, "Error deleting the request.");
            }

            var requestDto = existingRequest.ToRequestDto(); // Using RequestMapper
            return Ok(requestDto);
        }


        // DELETE: api/Requests/BySample/5
        [HttpDelete("BySample/{id}")]
        public async Task<ActionResult<RequestDto>> DeleteRequestBySample(int id)
        {
            var existingRequest = await _repository.GetRequestByIdAsync(id);
            if (existingRequest == null)
            {
                return NotFound("Request not found.");
            }

            if (existingRequest.Sample == null)
            {
                return BadRequest("This request does not have a sample associated with it.");
            }

            // Disassociate the Design before deleting the Request
            existingRequest.SampleId = null; // Set the foreign key to null
            existingRequest.Sample = null; // Set the navigation property to null

            // Save the changes to update the database
            var updateResult = await _repository.UpdateRequestAsync(existingRequest);
            if (!updateResult)
            {
                return StatusCode(500, "Error updating the request to disassociate the sample.");
            }

            // Proceed with the deletion
            var deleteResult = await _repository.DeleteRequestAsync(id);
            if (!deleteResult)
            {
                return StatusCode(500, "Error deleting the request.");
            }

            var requestDto = existingRequest.ToRequestDto(); // Using RequestMapper
            return Ok(requestDto);
        }
    }
}
