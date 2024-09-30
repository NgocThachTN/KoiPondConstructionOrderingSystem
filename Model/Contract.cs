using System;
using System.Collections.Generic;

namespace KoiPondConstructionManagement.Model;

public partial class Contract
{
    public int ContractId { get; set; }

    public int? RequestId { get; set; }

    public string? ContractName { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Status { get; set; }

    public string? Description { get; set; }

    public virtual Request? Request { get; set; }
}
