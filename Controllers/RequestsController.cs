
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KoiPond.Repositories;
using KoiPond.Models;
using KoiPond.DTOs;
using KoiPond.AutoMapper;

namespace KoiPond.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestsRepository _repository;
        private KoiContext _context;

        public RequestsController(IRequestsRepository repository, KoiContext context)
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

        // PUT: api/Requests/ByDesign/5
        [HttpPut("ByDesign/{id}")]
        public async Task<ActionResult<RequestDto>> PutRequestByDesign(int id, UpdateRequestDesignDto requestDto)
        {
            var existingRequest = await _repository.GetRequestByIdAsync(id);
            if (existingRequest == null)
            {
                return NotFound("Request not found.");
            }

            existingRequest.RequestName = requestDto.RequestName;
            existingRequest.Description = requestDto.Description;

            // Update the User and Account information
            existingRequest.User ??= new User();
            existingRequest.User.Name = requestDto.User.Name;
            existingRequest.User.PhoneNumber = requestDto.User.PhoneNumber;
            existingRequest.User.Address = requestDto.User.Address;
            existingRequest.User.Account ??= new Account();
            existingRequest.User.Account.UserName = requestDto.User.UserName;
            existingRequest.User.Account.Email = requestDto.User.Email;
            existingRequest.User.Account.Password = requestDto.User.Password;
            existingRequest.User.RoleId = requestDto.User.RoleId;

            if (requestDto.Design != null)
            {
                existingRequest.Design = new Design
                {
                    ConstructionType = new ConstructionType
                    {
                        ConstructionTypeName = requestDto.Design.ConstructionTypeName
                    },
                    DesignName = requestDto.Design.DesignName,
                    DesignSize = requestDto.Design.DesignSize,
                    DesignPrice = requestDto.Design.DesignPrice,
                    DesignImage = requestDto.Design.DesignImage
                };
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

            // Update the User and Account information
            existingRequest.User ??= new User();
            existingRequest.User.Name = requestDto.User.Name;
            existingRequest.User.PhoneNumber = requestDto.User.PhoneNumber;
            existingRequest.User.Address = requestDto.User.Address;
            existingRequest.User.Account ??= new Account();
            existingRequest.User.Account.UserName = requestDto.User.UserName;
            existingRequest.User.Account.Email = requestDto.User.Email;
            existingRequest.User.Account.Password = requestDto.User.Password;
            existingRequest.User.RoleId = requestDto.User.RoleId;

            // Update Sample information
            if (requestDto.Sample != null)
            {
                existingRequest.Sample = new Sample
                {
                    ConstructionType = new ConstructionType
                    {
                        ConstructionTypeName = requestDto.Sample.ConstructionTypeName
                    },
                    SampleName = requestDto.Sample.SampleName,
                    SampleSize = requestDto.Sample.SampleSize,
                    SamplePrice = requestDto.Sample.SamplePrice,
                    SampleImage = requestDto.Sample.SampleImage
                };
            }

            // Update the request in the database
            await _repository.UpdateRequestAsync(existingRequest);
            var requestResultDto = existingRequest.ToRequestDto(); // Using RequestMapper

            return Ok(requestResultDto);
        }


        // POST: api/Requests/ByDesign
        [HttpPost("ByDesign")]
        public async Task<ActionResult<RequestDto>> PostRequestByDesign(CreateRequestDesignDto requestDto)
        {
            if (!requestDto.IsDesignSelected)
            {
                return BadRequest("Design must be selected for this endpoint.");
            }

            var request = requestDto.ToRequestDesignFromCreatedDto();
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

            var request = requestDto.ToRequestSampleFromCreatedDto();
            await _repository.AddRequestAsync(request);
            var requestResultDto = request.ToRequestDto(); // Using RequestMapper
            return CreatedAtAction(nameof(GetRequest), new { id = request.RequestId }, requestResultDto);
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

            var result = await _repository.DeleteRequestAsync(id);
            if (!result)
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

            var result = await _repository.DeleteRequestAsync(id);
            if (!result)
            {
                return StatusCode(500, "Error deleting the request.");
            }

            var requestDto = existingRequest.ToRequestDto(); // Using RequestMapper
            return Ok(requestDto);
        }
    }
}
