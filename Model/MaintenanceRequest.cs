using System;
using System.Collections.Generic;

namespace KoiPondConstructionManagement.Model;

public partial class MaintenanceRequest
{
    public int MaintenanceId { get; set; }

    public int RequestId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Status { get; set; }

    public virtual Maintenance Maintenance { get; set; } = null!;

    public virtual Request Request { get; set; } = null!;
}
