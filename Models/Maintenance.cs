using System;
using System.Collections.Generic;

namespace KoiPond.Models;

public partial class Maintenance
{
    public int MaintenanceId { get; set; }

    public string? MaintencaceName { get; set; }

    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();
}
