using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hahi.ModelsV1;
using Hahi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Hahi.DTOs;
using System.Diagnostics.Contracts;
using Hahi.AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Hahi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MaintenanceRequestsController : ControllerBase
    {
        private readonly IMaintenanceRequestsRepository _repository;
        private KoisV1Context _context;

        public MaintenanceRequestsController(IMaintenanceRequestsRepository repository, KoisV1Context context)
        {
            _repository = repository;
            _context = context;
        }

        // GET: api/MaintenanceRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaintenanceRequestDto>>> GetMaintenanceRequests()
        {
            var maintenanceRequests = await _repository.GetMaintenanceRequestsAsync();
            var maintenanceRequestDtos = maintenanceRequests.Select(maintenanceRequest => maintenanceRequest.ToMaintenanceDto()).ToList();

            return Ok(maintenanceRequestDtos);
        }

        // GET: api/MaintenanceRequests/5/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MaintenanceRequestDto>> GetMaintenanceRequestById(int id)
        {
            var maintenanceRequest = await _repository.GetMaintenanceRequestByIdAsync(id);

            if (maintenanceRequest == null)
            {
                return NotFound();
            }

            var maintenanceRequestDto = maintenanceRequest.ToMaintenanceDto();
            return Ok(maintenanceRequestDto);
        }

        [HttpPost("ByMaintenanceRequestDesign")]
        public async Task<ActionResult<MaintenanceRequestDto>> PostMaintenanceByRequestDesign(CreateMaintenanceRequestDesignDto maintenanceRequestDto)
        {
            // Check if the Request already exists
            var existingRequest = await _context.Requests
                .FirstOrDefaultAsync(r => r.RequestName == maintenanceRequestDto.Requests.First().RequestName &&
                                          r.Description == maintenanceRequestDto.Requests.First().Description);
            if (existingRequest == null)
            {
                return BadRequest(new { message = "Request does not exist. Please provide an existing Request." });
            }

            // Check if the User already exists
            var existingUser = await _context.Users
                .Include(u => u.Account)
                .FirstOrDefaultAsync(u => u.Name == maintenanceRequestDto.Requests.First().Users.First().Name &&
                                          u.PhoneNumber == maintenanceRequestDto.Requests.First().Users.First().PhoneNumber &&
                                          u.Account.Email == maintenanceRequestDto.Requests.First().Users.First().Email &&
                                          u.Address == maintenanceRequestDto.Requests.First().Users.First().Address);
            if (existingUser == null)
            {
                return BadRequest(new { message = "User does not exist. Please provide an existing User." });
            }

            // Check if the Design already exists
            var existingDesign = await _context.Designs
                .FirstOrDefaultAsync(d => d.DesignName == maintenanceRequestDto.Requests.First().Designs.First().DesignName &&
                                          d.DesignSize == maintenanceRequestDto.Requests.First().Designs.First().DesignSize);
            if (existingDesign == null)
            {
                return BadRequest(new { message = "Design does not exist. Please provide an existing Design." });
            }

            // Check if the Maintenance already exists
            var existingMaintenance = await _context.Maintenances
                .FirstOrDefaultAsync(m => m.MaintencaceName == maintenanceRequestDto.Maintenance.First().MaintencaceName);
            if (existingMaintenance == null)
            {
                return BadRequest(new { message = "Maintenance does not exist. Please provide an existing Maintenance." });
            }

            // Create the MaintenanceRequest using the existing entities and their IDs
            var maintenanceRequest = new MaintenanceRequest
            {
                RequestId = existingRequest.RequestId, // Use the existing Request ID
                Request = existingRequest, // Use the existing Request object
                MaintenanceRequestStartDate = maintenanceRequestDto.MaintenanceRequestStartDate,
                MaintenanceRequestEndDate = maintenanceRequestDto.MaintenanceRequestEndDate,
                Status = maintenanceRequestDto.Status,
                MaintenanceRequestNavigation = existingMaintenance, // Use the existing Maintenance object
                MaintenanceRequestId = existingMaintenance.MaintenanceId // Use the existing Maintenance ID
            };

            // Ensure the User information is correctly linked without generating a new ID
            maintenanceRequest.Request.UserId = existingUser.UserId; // Use the existing User ID
            maintenanceRequest.Request.User = existingUser; // Use the existing User object

            // Ensure the Design information is correctly linked without generating a new ID
            maintenanceRequest.Request.DesignId = existingDesign.DesignId; // Use the existing Design ID
            maintenanceRequest.Request.Design = existingDesign; // Use the existing Design object

            // Add the MaintenanceRequest to the database
            _context.MaintenanceRequests.Add(maintenanceRequest);
            await _context.SaveChangesAsync(); // Save changes to the database

            // Convert the result to a DTO to return to the client
            var maintenanceResultDto = maintenanceRequest.ToMaintenanceDto();
            return CreatedAtAction(nameof(GetMaintenanceRequestById), new { id = maintenanceRequest.MaintenanceRequestId }, maintenanceResultDto);
        }


        [HttpPost("ByMaintenanceRequestSample")]
        public async Task<ActionResult<MaintenanceRequestDto>> PostMaintenanceByRequestSample(CreateMaintenanceRequestSampleDto maintenanceRequestDto)
        {
            // Check if the Request already exists
            var existingRequest = await _context.Requests
                .FirstOrDefaultAsync(r => r.RequestName == maintenanceRequestDto.Requests.First().RequestName &&
                                          r.Description == maintenanceRequestDto.Requests.First().Description);
            if (existingRequest == null)
            {
                return BadRequest(new { message = "Request does not exist. Please provide an existing Request." });
            }

            // Check if the User already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Name == maintenanceRequestDto.Requests.First().Users.First().Name &&
                                          u.PhoneNumber == maintenanceRequestDto.Requests.First().Users.First().PhoneNumber &&
                                          u.Account.Email == maintenanceRequestDto.Requests.First().Users.First().Email &&
                                          u.Address == maintenanceRequestDto.Requests.First().Users.First().Address);
            if (existingUser == null)
            {
                return BadRequest(new { message = "User does not exist. Please provide an existing User." });
            }

            // Check if the Design already exists
            var existingSample = await _context.Samples
                .FirstOrDefaultAsync(d => d.SampleName == maintenanceRequestDto.Requests.First().Samples.First().SampleName &&
                                          d.SampleSize == maintenanceRequestDto.Requests.First().Samples.First().SampleSize);
            if (existingSample == null)
            {
                return BadRequest(new { message = "Sample does not exist. Please provide an existing Sample." });
            }

            // Check if the Maintenance already exists
            var existingMaintenance = await _context.Maintenances
                .FirstOrDefaultAsync(m => m.MaintencaceName == maintenanceRequestDto.Maintenance.First().MaintencaceName);
            if (existingMaintenance == null)
            {
                return BadRequest(new { message = "Maintenance does not exist. Please provide an existing Maintenance." });
            }

            // Create the MaintenanceRequest using the existing entities and their IDs
            var maintenanceRequest = new MaintenanceRequest
            {
                RequestId = existingRequest.RequestId, // Use the existing Request ID
                Request = existingRequest, // Use the existing Request object
                MaintenanceRequestStartDate = maintenanceRequestDto.MaintenanceRequestStartDate,
                MaintenanceRequestEndDate = maintenanceRequestDto.MaintenanceRequestEndDate,
                Status = maintenanceRequestDto.Status,
                MaintenanceRequestNavigation = existingMaintenance, // Use the existing Maintenance object
                MaintenanceRequestId = existingMaintenance.MaintenanceId // Use the existing Maintenance ID
            };

            // Ensure the User information is correctly linked without generating a new ID
            maintenanceRequest.Request.UserId = existingUser.UserId; // Use the existing User ID
            maintenanceRequest.Request.User = existingUser; // Use the existing User object

            // Ensure the Design information is correctly linked without generating a new ID
            maintenanceRequest.Request.SampleId = existingSample.SampleId; // Use the existing Design ID
            maintenanceRequest.Request.Sample = existingSample; // Use the existing Design object

            // Add the MaintenanceRequest to the database
            _context.MaintenanceRequests.Add(maintenanceRequest);
            await _context.SaveChangesAsync(); // Save changes to the database

            // Convert the result to a DTO to return to the client
            var maintenanceResultDto = maintenanceRequest.ToMaintenanceDto();
            return CreatedAtAction(nameof(GetMaintenanceRequestById), new { id = maintenanceRequest.MaintenanceRequestId }, maintenanceResultDto);
        }



        [HttpPut("ByMaintenanceRequestDesign/{id}")]
        public async Task<ActionResult> UpdateMaintenanceByRequestDesign(int id, CreateMaintenanceRequestDesignDto maintenanceDto)
        {
            var existingMaintenance = await _context.MaintenanceRequests
                .Include(c => c.Request)
                    .ThenInclude(r => r.Design)
                .Include(c => c.Request.User)
                .Include(c => c.MaintenanceRequestNavigation)
                .FirstOrDefaultAsync(c => c.MaintenanceRequestId == id);

            if (existingMaintenance == null)
            {
                return NotFound("Maintenance not found.");
            }

            // Update maintenance request details
            existingMaintenance.MaintenanceRequestStartDate = maintenanceDto.MaintenanceRequestStartDate;
            existingMaintenance.MaintenanceRequestEndDate = maintenanceDto.MaintenanceRequestEndDate;
            existingMaintenance.Status = maintenanceDto.Status;

            // Update Request details with User information
            if (maintenanceDto.Requests != null && maintenanceDto.Requests.Count > 0)
            {
                var requestSampleDto = maintenanceDto.Requests.First();

                if (existingMaintenance.Request != null)
                {
                    existingMaintenance.Request.RequestName = requestSampleDto.RequestName;
                    existingMaintenance.Request.Description = requestSampleDto.Description;

                    // Update User details in the request
                    if (requestSampleDto.Users != null && requestSampleDto.Users.Count > 0)
                    {
                        var userDto = requestSampleDto.Users.First();
                        if (existingMaintenance.Request.User != null)
                        {
                            existingMaintenance.Request.User.Name = userDto.Name;
                            existingMaintenance.Request.User.PhoneNumber = userDto.PhoneNumber;
                            existingMaintenance.Request.User.Address = userDto.Address;

                            if (existingMaintenance.Request.User.Account != null)
                            {
                                existingMaintenance.Request.User.Account.UserName = userDto.UserName;
                                existingMaintenance.Request.User.Account.Email = userDto.Email;
                                existingMaintenance.Request.User.Account.Password = userDto.Password;
                            }
                            existingMaintenance.Request.User.RoleId = userDto.RoleId;
                        }
                    }

                    // Update the Design details in the request
                    if (requestSampleDto.Designs != null && requestSampleDto.Designs.Count > 0)
                    {
                        var designDto = requestSampleDto.Designs.First();
                        if (existingMaintenance.Request.Design != null)
                        {
                            existingMaintenance.Request.Design.DesignName = designDto.DesignName;
                            existingMaintenance.Request.Design.DesignSize = designDto.DesignSize;
                            existingMaintenance.Request.Design.DesignPrice = designDto.DesignPrice;
                            existingMaintenance.Request.Design.DesignImage = designDto.DesignImage;
                        }
                    }
                }
            }

            // Update Maintenance details
            if (maintenanceDto.Maintenance != null && maintenanceDto.Maintenance.Count > 0)
            {
                var maintenanceDtoItem = maintenanceDto.Maintenance.First();
                if (existingMaintenance.MaintenanceRequestNavigation != null)
                {
                    existingMaintenance.MaintenanceRequestNavigation.MaintencaceName = maintenanceDtoItem.MaintencaceName;
                }
            }
            // Ensure that the entity state is set to Modified
            _context.Entry(existingMaintenance).State = EntityState.Modified;

            // Save changes to the database
            try
            {
                await _context.SaveChangesAsync();
                return Ok("MaintenanceRequest updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPut("ByMaintenanceRequestSample/{id}")]
        public async Task<ActionResult> UpdateMaintenanceByRequestSample(int id, CreateMaintenanceRequestSampleDto maintenanceDto)
        {
            var existingMaintenance = await _context.MaintenanceRequests
                .Include(c => c.Request)
                    .ThenInclude(r => r.Sample)
                .Include(c => c.Request.User)
                .Include(c => c.MaintenanceRequestNavigation)
                .FirstOrDefaultAsync(c => c.MaintenanceRequestId == id);

            if (existingMaintenance == null)
            {
                return NotFound("Maintenance not found.");
            }

            // Update maintenance request details
            existingMaintenance.MaintenanceRequestStartDate = maintenanceDto.MaintenanceRequestStartDate;
            existingMaintenance.MaintenanceRequestEndDate = maintenanceDto.MaintenanceRequestEndDate;
            existingMaintenance.Status = maintenanceDto.Status;

            // Update Request details with User information
            if (maintenanceDto.Requests != null && maintenanceDto.Requests.Count > 0)
            {
                var requestSampleDto = maintenanceDto.Requests.First();

                if (existingMaintenance.Request == null)
                {
                    existingMaintenance.Request = new Request(); // Create a new Request only if it does not exist
                }

                existingMaintenance.Request.RequestName = requestSampleDto.RequestName;
                existingMaintenance.Request.Description = requestSampleDto.Description;

                // Update User details in the request
                if (requestSampleDto.Users != null && requestSampleDto.Users.Count > 0)
                {
                    var userDto = requestSampleDto.Users.First();
                    existingMaintenance.Request.User ??= new User(); // Only create a new User if one doesn't exist already
                    existingMaintenance.Request.User.Name = userDto.Name;
                    existingMaintenance.Request.User.PhoneNumber = userDto.PhoneNumber;
                    existingMaintenance.Request.User.Address = userDto.Address;
                    existingMaintenance.Request.User.Account ??= new Account(); // Only create a new Account if one doesn't exist
                    existingMaintenance.Request.User.Account.UserName = userDto.UserName;
                    existingMaintenance.Request.User.Account.Email = userDto.Email;
                    existingMaintenance.Request.User.Account.Password = userDto.Password;
                    existingMaintenance.Request.User.RoleId = userDto.RoleId;
                }

                // Update the Sample details in the request
                if (requestSampleDto.Samples != null && requestSampleDto.Samples.Count > 0)
                {
                    existingMaintenance.Request.Sample = requestSampleDto.Samples.Select(sampleDto => new Sample
                    {
                        SampleName = sampleDto.SampleName,
                        SampleSize = sampleDto.SampleSize,
                        SamplePrice = sampleDto.SamplePrice,
                        SampleImage = sampleDto.SampleImage
                    }).FirstOrDefault();
                }
            }

            // Update Maintenance details
            if (maintenanceDto.Maintenance != null && maintenanceDto.Maintenance.Count > 0)
            {
                var maintenanceDtoItem = maintenanceDto.Maintenance.First();

                if (existingMaintenance.MaintenanceRequestNavigation == null)
                {
                    existingMaintenance.MaintenanceRequestNavigation = new Maintenance(); // Only create a new Maintenance if it doesn't exist
                }

                existingMaintenance.MaintenanceRequestNavigation.MaintencaceName = maintenanceDtoItem.MaintencaceName;
            }

            // Save changes to the database
            try
            {
                await _context.SaveChangesAsync();
                return Ok("MaintenanceRequest updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpDelete("ByRequestDesign/{id}")]
        public async Task<IActionResult> DeleteMaintenanceByRequestDesign(int id)
        {
            // Retrieve the existing maintenance request with the specified ID
            var existingMaintenance = await _context.MaintenanceRequests
                .Include(c => c.Request)
                    .ThenInclude(r => r.Design) // Include Design to check if it exists
                .Include(c => c.Request.Sample) // Include Sample to check if it exists
                .Include(c => c.Request.User)
                .Include(c => c.MaintenanceRequestNavigation)
                .FirstOrDefaultAsync(c => c.MaintenanceRequestId == id);

            if (existingMaintenance == null)
            {
                return NotFound("Maintenance not found.");
            }

            // Check if the request contains a SampleId, since we only want to delete if it has a DesignId
            if (existingMaintenance.Request != null && existingMaintenance.Request.Sample != null)
            {
                return BadRequest("Cannot delete Maintenance because it contains a request with a SampleId.");
            }

            // Delete the associated Request if it exists
            if (existingMaintenance.Request != null)
            {
                // Check if the Design exists and delete it first
                if (existingMaintenance.Request.Design != null)
                {
                    _context.Designs.Remove(existingMaintenance.Request.Design);
                }

                // Delete the Request itself
                _context.Requests.Remove(existingMaintenance.Request);
            }

            // Delete the MaintenanceRequest itself
            _context.MaintenanceRequests.Remove(existingMaintenance);

            // Save changes to the database
            try
            {
                await _context.SaveChangesAsync();
                return Ok("MaintenanceRequest, its associated Request, and Design were successfully deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpDelete("ByRequestSample/{id}")]
        public async Task<IActionResult> DeleteMaintenanceByRequestSample(int id)
        {
            var existingMaintenance = await _context.MaintenanceRequests
                .Include(c => c.Request)
                    .ThenInclude(r => r.Sample)
                .Include(c => c.Request.Design)
                .Include(c => c.Request.User)
                .Include(c => c.MaintenanceRequestNavigation)
                .FirstOrDefaultAsync(c => c.MaintenanceRequestId == id);

            if (existingMaintenance == null)
            {
                return NotFound("Maintenance not found.");
            }

            // Check if the request contains a DesignId, since we only want to delete if it has a SampleId
            if (existingMaintenance.Request != null && existingMaintenance.Request.Design != null)
            {
                return BadRequest("Cannot delete Maintenance because it contains a request with a DesignId.");
            }

            // Delete the associated request if it contains a SampleId
            if (existingMaintenance.Request != null && existingMaintenance.Request.Sample != null)
            {
                _context.Requests.Remove(existingMaintenance.Request);
            }

            // Delete the contract itself
            _context.MaintenanceRequests.Remove(existingMaintenance);

            // Save changes to the database
            try
            {
                await _context.SaveChangesAsync();
                return Ok("MaintenanceRequest and its associated request with SampleId were successfully deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
