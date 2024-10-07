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
            var contract = contractDto.ToContractDesignFromCreatedDto(); // Using ContractMapper
            await _repository.AddContractAsync(contract);
            var contractResultDto = contract.ToContractDto();
            return CreatedAtAction(nameof(GetContractById), new { id = contract.ContractId }, contractResultDto);
        }


        // POST: api/Contracts/BySampleDesign
        [HttpPost("BySampleDesign")]
        public async Task<ActionResult<ContractDto>> PostContractBySampleDesign(CreateContractSampleDto contractDto)
        {
            var contract = contractDto.ToContractSampleFromCreatedDto(); // Using ContractMapper
            await _repository.AddContractAsync(contract);
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

            // Update contract details
            existingContract.ContractName = contractDto.ContractName;
            existingContract.ContractStartDate = contractDto.ContractStartDate;
            existingContract.ContractEndDate = contractDto.ContractEndDate;
            existingContract.Status = contractDto.Status;
            existingContract.Description = contractDto.Description;

            // Update Request details with User information
            if (contractDto.Requests != null && contractDto.Requests.Count > 0)
            {
                var requestSampleDto = contractDto.Requests.First();

                if (existingContract.Request == null)
                {
                    existingContract.Request = new Request();
                }

                existingContract.Request.RequestName = requestSampleDto.RequestName;
                existingContract.Request.Description = requestSampleDto.Description;

                // Update User details in the request
                if (requestSampleDto.Users != null && requestSampleDto.Users.Count > 0)
                {
                    var userDto = requestSampleDto.Users.First();
                    existingContract.Request.User ??= new User();
                    existingContract.Request.User.Name = userDto.Name;
                    existingContract.Request.User.PhoneNumber = userDto.PhoneNumber;
                    existingContract.Request.User.Address = userDto.Address;
                    existingContract.Request.User.RoleId = userDto.RoleId;

                    // Assuming you have an Account field in User
                    existingContract.Request.User.Account ??= new Account();
                    existingContract.Request.User.Account.UserName = userDto.UserName;
                    existingContract.Request.User.Account.Email = userDto.Email;
                    existingContract.Request.User.Account.Password = userDto.Password;
                }

                // Update the Sample details in the request
                if (requestSampleDto.Designs != null && requestSampleDto.Designs.Count > 0)
                {
                    existingContract.Request.Design = requestSampleDto.Designs.Select(sampleDto => new Design
                    {
                        DesignName = sampleDto.DesignName,
                        DesignSize = sampleDto.DesignSize,
                        DesignPrice = sampleDto.DesignPrice,
                        DesignImage = sampleDto.DesignImage
                    }).FirstOrDefault();
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

            // Update contract details
            existingContract.ContractName = contractDto.ContractName;
            existingContract.ContractStartDate = contractDto.ContractStartDate;
            existingContract.ContractEndDate = contractDto.ContractEndDate;
            existingContract.Status = contractDto.Status;
            existingContract.Description = contractDto.Description;

            // Update Request details with User information
            if (contractDto.Requests != null && contractDto.Requests.Count > 0)
            {
                var requestSampleDto = contractDto.Requests.First();

                if (existingContract.Request == null)
                {
                    existingContract.Request = new Request();
                }

                existingContract.Request.RequestName = requestSampleDto.RequestName;
                existingContract.Request.Description = requestSampleDto.Description;

                // Update User details in the request
                if (requestSampleDto.Users != null && requestSampleDto.Users.Count > 0)
                {
                    var userDto = requestSampleDto.Users.First();
                    existingContract.Request.User ??= new User();
                    existingContract.Request.User.Name = userDto.Name;
                    existingContract.Request.User.PhoneNumber = userDto.PhoneNumber;
                    existingContract.Request.User.Address = userDto.Address;
                    existingContract.Request.User.RoleId = userDto.RoleId;

                    // Assuming you have an Account field in User
                    existingContract.Request.User.Account ??= new Account();
                    existingContract.Request.User.Account.UserName = userDto.UserName;
                    existingContract.Request.User.Account.Email = userDto.Email;
                    existingContract.Request.User.Account.Password = userDto.Password;
                }

                // Update the Sample details in the request
                if (requestSampleDto.Samples != null && requestSampleDto.Samples.Count > 0)
                {
                    existingContract.Request.Sample = requestSampleDto.Samples.Select(sampleDto => new Sample
                    {
                        SampleName = sampleDto.SampleName,
                        SampleSize = sampleDto.SampleSize,
                        SamplePrice = sampleDto.SamplePrice,
                        SampleImage = sampleDto.SampleImage
                    }).FirstOrDefault();
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
                .ThenInclude(r => r.Sample) // Include Sample to check if it exists
                .Include(c => c.Request.Design) // Include Design to check if it exists
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
    }
}
