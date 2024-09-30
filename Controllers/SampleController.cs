using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using KoiPondConstructionManagement.Model;
using Microsoft.AspNetCore.Mvc;

namespace KoiPondConstructionManagement.Controllers
{
    [Route("api/sample")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly PrototypeKoiv1Context _context;
        public SampleController(PrototypeKoiv1Context context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult getAll()
        {
            var samples = _context.Samples.ToList();
            return Ok(samples);
        }
        [HttpGet("{SampleID}")]
        public IActionResult GetBySampleId([FromRoute] int SampleID)
        {
            var sample = _context.Samples.Find(SampleID);
            if(sample == null){
                return NotFound();
            }
            return Ok(sample);
        }
    }
}