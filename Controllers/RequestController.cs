using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiPondConstructionManagement.Dtos.Request;
using KoiPondConstructionManagement.Mappers;
using KoiPondConstructionManagement.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoiPondConstructionManagement.Controllers
{
    [Route("api/requests")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly PrototypeKoiv1Context _context;

        public RequestController(PrototypeKoiv1Context context)
        {
            _context = context; 
        }
        [HttpGet]
        [Authorize]
        public IActionResult GetAll()
        {
            var requests = _context.Requests.ToList()
            .Select(s => s.ToRequestDto());
            return Ok(requests);
        }
        [HttpGet("{RequestID}")]
        [Authorize]
        public IActionResult GetById([FromRoute] int RequestID)
        {
            var request = _context.Requests.Find(RequestID);
            if (request == null)
            {
                return NotFound();
            }
            return Ok(request.ToRequestDto());
        }


    }
}