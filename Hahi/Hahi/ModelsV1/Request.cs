using System;
using System.Collections.Generic;

namespace Hahi.ModelsV1
{
    public partial class Request
    {
        public Request()
        {
            MaintenanceRequests = new HashSet<MaintenanceRequest>();
        }

        public int RequestId { get; set; }
        public int? UserId { get; set; }
        public string? RequestName { get; set; }
        public string? Description { get; set; }
        public int? SampleId { get; set; }
        public int? DesignId { get; set; }

        public virtual Design? Design { get; set; }
        public virtual Sample? Sample { get; set; }
        public virtual User? User { get; set; }
        public virtual Contract? Contract { get; set; }
        public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; }
    }
}
