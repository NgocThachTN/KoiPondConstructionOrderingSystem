using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiPondConstructionManagement.Dtos.Request
{
    public class RequestDto
    {
        public int RequestId { get; set; }

        public string? RequestName { get; set; }

        public string? Description { get; set; }


    }
}