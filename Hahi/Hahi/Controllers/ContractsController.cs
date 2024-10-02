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
    public class ContractsController : ControllerBase
    {
        private readonly IContractsRepository _repository;

        public ContractsController(IContractsRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Contracts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contract>>> GetContracts()
        {
            var contracts = await _repository.GetContractsAsync();
            return Ok(contracts);
        }

        // GET: api/Contracts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contract>> GetContract(int id)
        {
            var contract = await _repository.GetContractByIdAsync(id);

            if (contract == null)
            {
                return NotFound();
            }

            return Ok(contract);
        }

        // PUT: api/Contracts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContract(int id, Contract contract)
        {
            if (id != contract.ContractId)
            {
                return BadRequest();
            }

            try
            {
                await _repository.UpdateContractAsync(contract);
            }
            catch
            {
                if (!await _repository.ContractExistsAsync(id))
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

        // POST: api/Contracts
        [HttpPost]
        public async Task<ActionResult<Contract>> PostContract(Contract contract)
        {
            await _repository.AddContractAsync(contract);
            return CreatedAtAction(nameof(GetContract), new { id = contract.ContractId }, contract);
        }

        // DELETE: api/Contracts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContract(int id)
        {
            var contract = await _repository.GetContractByIdAsync(id);
            if (contract == null)
            {
                return NotFound();
            }

            await _repository.DeleteContractAsync(id);
            return NoContent();
        }
    }
}
