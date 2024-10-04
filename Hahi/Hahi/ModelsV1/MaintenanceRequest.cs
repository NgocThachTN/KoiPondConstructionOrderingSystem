using System;
using System.Collections.Generic;

namespace Hahi.ModelsV1
{
    public partial class MaintenanceRequest
    {
        public int MaintenanceRequestId { get; set; }
        public int RequestId { get; set; }
        public DateTime? MaintenanceRequestStartDate { get; set; }
        public DateTime? MaintenanceRequestEndDate { get; set; }
        public string? Status { get; set; }

        public virtual Maintenance MaintenanceRequestNavigation { get; set; } = null!;
        public virtual Request Request { get; set; } = null!;
    }
}
