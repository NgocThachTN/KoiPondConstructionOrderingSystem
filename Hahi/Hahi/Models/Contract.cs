using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hahi.Models
{
    public partial class Contract
    {
        public int ContractId { get; set; }
        public int? RequestId { get; set; }
        [Required]
        public string? ContractName { get; set; }
        [Required]
        public DateTime? StartDate { get; set; }
        [Required]
        public DateTime? EndDate { get; set; }
        [Required]
        public string? Status { get; set; }
        [Required]
        public string? Description { get; set; }

        public virtual Request? Request { get; set; }
    }
}
