using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hahi.ModelsV1;
using Hahi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Hahi.DTOs;  // Include this to access the DTOs
using Hahi.AutoMapper;
using Microsoft.EntityFrameworkCore;  // Include this to access ContractMapper and RequestMapper

namespace Hahi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContractsController : ControllerBase
    {
        private readonly IContractsRepository _repository;
        private readonly KoisV1Context _context;

        public ContractsController(IContractsRepository repository, KoisV1Context context)
        {
            _repository = repository;
            _context = context;
        }

        // GET: api/Contracts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContractDto>>> GetAll()
        {
            var contracts = await _repository.GetContractsAsync();
            var contractDtos = contracts.Select(contract => contract.ToContractDto()).ToList();

            return Ok(contractDtos);
        }



        // GET: api/Contracts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContractDto>> GetContractById(int id)
        {
            var contract = await _repository.GetContractByIdAsync(id);

            if (contract == null)
            {
                return NotFound();
            }

            var contractDto = contract.ToContractDto();
            return Ok(contractDto);
        }


        // POST: api/Contracts/ByRequestDesign
        [HttpPost("ByRequestDesign")]
        public async Task<ActionResult<ContractDto>> PostContractByRequestDesign(CreateContractDesignDto contractDto)
        {
            // Check if the Design already exists in the database
            var design = await _context.Designs
                .FirstOrDefaultAsync(d => d.DesignName == contractDto.Requests.First().Designs.First().DesignName &&
                                          d.DesignSize == contractDto.Requests.First().Designs.First().DesignSize);

            if (design == null)
            {
                return BadRequest(new { message = "Design does not exist. Please provide an existing Design." });
            }

            // Check if the ConstructionType exists for the Design
            var constructionType = await _context.ConstructionTypes
                .FirstOrDefaultAsync(ct => ct.ConstructionTypeName == contractDto.Requests.First().Designs.First().ConstructionTypeName);

            if (constructionType == null)
            {
                return BadRequest(new { message = "ConstructionType does not exist. Please provide an existing ConstructionType." });
            }

            // Check if the User exists in the database
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name == contractDto.Requests.First().Users.First().Name &&
                                          u.PhoneNumber == contractDto.Requests.First().Users.First().PhoneNumber &&
                                          u.Account.Email == contractDto.Requests.First().Users.First().Email &&
                                          u.Address == contractDto.Requests.First().Users.First().Address);

            if (user == null)
            {
                return BadRequest(new { message = "User does not exist. Please provide an existing User." });
            }

            // Check if the Request already exists in the database
            var request = await _context.Requests
                .FirstOrDefaultAsync(r => r.RequestName == contractDto.Requests.First().RequestName &&
                                          r.Description == contractDto.Requests.First().Description);

            if (request == null)
            {
                return BadRequest(new { message = "Request does not exist. Please provide an existing Request." });
            }

            // Check if a Contract with the same unique data already exists
            var existingContract = await _context.Contracts
                .FirstOrDefaultAsync(c => c.ContractName == contractDto.ContractName &&
                                          c.ContractStartDate == contractDto.ContractStartDate &&
                                          c.ContractEndDate == contractDto.ContractEndDate);

            if (existingContract != null)
            {
                return Conflict(new { message = "A contract with the same details already exists." });
            }

            // If all related entities already exist, proceed to create the contract without creating new records
            var contract = contractDto.ToContractDesignFromCreatedDto(request, user, design);
            _context.Entry(request).State = EntityState.Unchanged;
            _context.Entry(user).State = EntityState.Unchanged;
            _context.Entry(design).State = EntityState.Unchanged;

            await _repository.AddContractAsync(contract);
            await _context.SaveChangesAsync(); // Save changes to the database

            var contractResultDto = contract.ToContractDto();
            return CreatedAtAction(nameof(GetContractById), new { id = contract.ContractId }, contractResultDto);
        }


        // POST: api/Contracts/BySampleDesign
        [HttpPost("BySampleDesign")]
        public async Task<ActionResult<ContractDto>> PostContractBySampleDesign(CreateContractSampleDto contractDto)
        {
            // Check if the Design already exists in the database
            var sample = await _context.Samples
                .FirstOrDefaultAsync(d => d.SampleName == contractDto.Requests.First().Samples.First().SampleName &&
                                          d.SampleSize == contractDto.Requests.First().Samples.First().SampleSize);

            if (sample == null)
            {
                return BadRequest(new { message = "Sample does not exist. Please provide an existing Sample." });
            }

            // Check if the ConstructionType exists for the Design
            var constructionType = await _context.ConstructionTypes
                .FirstOrDefaultAsync(ct => ct.ConstructionTypeName == contractDto.Requests.First().Samples.First().ConstructionTypeName);

            if (constructionType == null)
            {
                return BadRequest(new { message = "ConstructionType does not exist. Please provide an existing ConstructionType." });
            }

            // Check if the User exists in the database
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name == contractDto.Requests.First().Users.First().Name &&
                                          u.PhoneNumber == contractDto.Requests.First().Users.First().PhoneNumber &&
                                          u.Account.Email == contractDto.Requests.First().Users.First().Email &&
                                          u.Address == contractDto.Requests.First().Users.First().Address);

            if (user == null)
            {
                return BadRequest(new { message = "User does not exist. Please provide an existing User." });
            }

            // Check if the Request already exists in the database
            var request = await _context.Requests
                .FirstOrDefaultAsync(r => r.RequestName == contractDto.Requests.First().RequestName &&
                                          r.Description == contractDto.Requests.First().Description);

            if (request == null)
            {
                return BadRequest(new { message = "Request does not exist. Please provide an existing Request." });
            }

            // If all related entities already exist, proceed to create the contract without creating new records
            var contract = contractDto.ToContractSampleFromCreatedDto(request, user, sample);
            await _repository.AddContractAsync(contract);
            await _context.SaveChangesAsync(); // Save changes to the database

            var contractResultDto = contract.ToContractDto();
            return CreatedAtAction(nameof(GetContractById), new { id = contract.ContractId }, contractResultDto);
        }


        // PUT: api/Contracts/ByRequestDesign/5
        [HttpPut("ByRequestDesign/{id}")]
        public async Task<IActionResult> UpdateContractByRequestDesign(int id, CreateContractDesignDto contractDto)
        {
            var existingContract = await _context.Contracts
                .Include(c => c.Request)
                    .ThenInclude(r => r.Design)
                .Include(c => c.Request.User)
                .FirstOrDefaultAsync(c => c.ContractId == id);

            if (existingContract == null)
            {
                return NotFound("Contract not found.");
            }

            // Update contract details without creating a new Contract ID
            existingContract.ContractName = contractDto.ContractName;
            existingContract.ContractStartDate = contractDto.ContractStartDate;
            existingContract.ContractEndDate = contractDto.ContractEndDate;
            existingContract.Status = contractDto.Status;
            existingContract.Description = contractDto.Description;

            // Update Request details without creating a new Request ID
            if (contractDto.Requests != null && contractDto.Requests.Count > 0)
            {
                var requestDto = contractDto.Requests.First();

                if (existingContract.Request != null)
                {
                    existingContract.Request.RequestName = requestDto.RequestName;
                    existingContract.Request.Description = requestDto.Description;
                }

                // Update User details without creating a new User ID
                if (requestDto.Users != null && requestDto.Users.Count > 0)
                {
                    var userDto = requestDto.Users.First();
                    if (existingContract.Request.User != null)
                    {
                        existingContract.Request.User.Name = userDto.Name;
                        existingContract.Request.User.PhoneNumber = userDto.PhoneNumber;
                        existingContract.Request.User.Address = userDto.Address;

                        if (existingContract.Request.User.Account != null)
                        {
                            existingContract.Request.User.Account.UserName = userDto.UserName;
                            existingContract.Request.User.Account.Email = userDto.Email;
                            existingContract.Request.User.Account.Password = userDto.Password;
                        }

                        existingContract.Request.User.RoleId = userDto.RoleId;
                    }
                }

                // Update Design details without creating a new Design ID
                if (requestDto.Designs != null && requestDto.Designs.Count > 0)
                {
                    var designDto = requestDto.Designs.First();
                    if (existingContract.Request.Design != null)
                    {
                        existingContract.Request.Design.DesignName = designDto.DesignName;
                        existingContract.Request.Design.DesignSize = designDto.DesignSize;
                        existingContract.Request.Design.DesignPrice = designDto.DesignPrice;
                        existingContract.Request.Design.DesignImage = designDto.DesignImage;
                    }
                }
            }

            // Save changes to the database
            try
            {
                await _context.SaveChangesAsync();
                return Ok("Contract updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        // PUT: api/Contracts/BySampleDesign/5
        [HttpPut("ByRequestSample/{id}")]
        public async Task<IActionResult> UpdateContractByRequestSample(int id, CreateContractSampleDto contractDto)
        {
            // Check if the contract exists
            var existingContract = await _context.Contracts
                .Include(c => c.Request)
                    .ThenInclude(r => r.Sample)
                .Include(c => c.Request.User)
                .FirstOrDefaultAsync(c => c.ContractId == id);

            if (existingContract == null)
            {
                return NotFound("Contract not found.");
            }

            // Update contract details without creating a new Contract ID
            existingContract.ContractName = contractDto.ContractName;
            existingContract.ContractStartDate = contractDto.ContractStartDate;
            existingContract.ContractEndDate = contractDto.ContractEndDate;
            existingContract.Status = contractDto.Status;
            existingContract.Description = contractDto.Description;

            // Update Request details without creating a new Request ID
            if (contractDto.Requests != null && contractDto.Requests.Count > 0)
            {
                var requestDto = contractDto.Requests.First();

                if (existingContract.Request != null)
                {
                    existingContract.Request.RequestName = requestDto.RequestName;
                    existingContract.Request.Description = requestDto.Description;
                }

                // Update User details without creating a new User ID
                if (requestDto.Users != null && requestDto.Users.Count > 0)
                {
                    var userDto = requestDto.Users.First();
                    if (existingContract.Request.User != null)
                    {
                        existingContract.Request.User.Name = userDto.Name;
                        existingContract.Request.User.PhoneNumber = userDto.PhoneNumber;
                        existingContract.Request.User.Address = userDto.Address;

                        if (existingContract.Request.User.Account != null)
                        {
                            existingContract.Request.User.Account.UserName = userDto.UserName;
                            existingContract.Request.User.Account.Email = userDto.Email;
                            existingContract.Request.User.Account.Password = userDto.Password;
                        }

                        existingContract.Request.User.RoleId = userDto.RoleId;
                    }
                }

                // Update Design details without creating a new Design ID
                if (requestDto.Samples != null && requestDto.Samples.Count > 0)
                {
                    var sampleDto = requestDto.Samples.First();
                    if (existingContract.Request.Sample != null)
                    {
                        existingContract.Request.Sample.SampleName = sampleDto.SampleName;
                        existingContract.Request.Sample.SampleSize = sampleDto.SampleSize;
                        existingContract.Request.Sample.SamplePrice = sampleDto.SamplePrice;
                        existingContract.Request.Sample.SampleImage = sampleDto.SampleImage;
                    }
                }
            }

            // Save changes to the database
            try
            {
                await _context.SaveChangesAsync();
                return Ok("Contract updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // DELETE: api/Contracts/ByRequestDesign/5
        [HttpDelete("ByRequestDesign/{id}")]
        public async Task<IActionResult> DeleteContractByRequestDesign(int id)
        {
            // Retrieve the existing contract with the specified ID
            var existingContract = await _context.Contracts
                .Include(c => c.Request)
                .ThenInclude(r => r.Design) // Include Design to check if it exists
                .Include(c => c.Request.Sample) // Include Sample to check if it exists
                .FirstOrDefaultAsync(c => c.ContractId == id);

            if (existingContract == null)
            {
                return NotFound("Contract not found.");
            }

            // Check if the request contains a SampleId
            if (existingContract.Request != null && existingContract.Request.Sample != null)
            {
                return BadRequest("Cannot delete contract because it contains a request with a SampleId.");
            }

            // Delete the associated request if it contains a DesignId
            if (existingContract.Request != null && existingContract.Request.Design != null)
            {
                _context.Requests.Remove(existingContract.Request);
            }

            // Delete the contract itself
            _context.Contracts.Remove(existingContract);

            // Save changes to the database
            try
            {
                await _context.SaveChangesAsync();
                return Ok("Contract and its associated request with DesignId were successfully deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpDelete("ByRequestSample/{id}")]
        public async Task<IActionResult> DeleteContractByRequestSample(int id)
        {
            // Retrieve the existing contract with the specified ID
            var existingContract = await _context.Contracts
                .Include(c => c.Request)
                .ThenInclude(r => r.Sample) 
                .Include(c => c.Request.Design) 
                .FirstOrDefaultAsync(c => c.ContractId == id);

            if (existingContract == null)
            {
                return NotFound("Contract not found.");
            }

            // Check if the request contains a DesignId
            if (existingContract.Request != null && existingContract.Request.Design != null)
            {
                return BadRequest("Cannot delete contract because it contains a request with a DesignId.");
            }

            // Delete the associated request if it contains a SampleId
            if (existingContract.Request != null && existingContract.Request.Sample != null)
            {
                _context.Requests.Remove(existingContract.Request);
            }

            // Delete the contract itself
            _context.Contracts.Remove(existingContract);

            // Save changes to the database
            try
            {
                await _context.SaveChangesAsync();
                return Ok("Contract and its associated request with SampleId were successfully deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private bool ContractExists(int id)
        {
            return _context.Contracts.Any(e => e.ContractId == id);
        }
    }
}
