﻿
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
    public class MaintenanceRequestsController : ControllerBase
    {
        private readonly IMaintenanceRequestsRepository _repository;
        private KoiContext _context;

        public MaintenanceRequestsController(IMaintenanceRequestsRepository repository, KoiContext context)
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


        // POST: api/MaintenanceRequests
        [HttpPost("ByMaintenanceRequestDesign")]
        public async Task<ActionResult<MaintenanceRequestDto>> PostMaintenanceByRequestDesign(CreateMaintenanceRequestDesignDto maintenanceRequestDto)
        {
            var mainenanceRequest = maintenanceRequestDto.ToMaintenanceRequestDesignFromCreatedDto(); // Using ContractMapper
            await _repository.AddMaintenanceRequestAsync(mainenanceRequest);
            var mainenanceResultDto = mainenanceRequest.ToMaintenanceDto();
            return CreatedAtAction(nameof(GetMaintenanceRequestById), new { id = mainenanceRequest.MaintenanceRequestId }, mainenanceResultDto);
        }

        [HttpPost("ByMaintenanceRequestSample")]
        public async Task<ActionResult<MaintenanceRequestDto>> PostMaintenanceByRequestSample(CreateMaintenanceRequestSampleDto maintenanceRequestDto)
        {
            var mainenanceRequest = maintenanceRequestDto.ToMaintenanceRequestSampleFromCreatedDto(); // Using ContractMapper
            await _repository.AddMaintenanceRequestAsync(mainenanceRequest);
            var mainenanceResultDto = mainenanceRequest.ToMaintenanceDto();
            return CreatedAtAction(nameof(GetMaintenanceRequestById), new { id = mainenanceRequest.MaintenanceRequestId }, mainenanceResultDto);
        }
    }
}
