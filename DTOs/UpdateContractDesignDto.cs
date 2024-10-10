namespace KoiPond.DTOs
{
    public class UpdateContractDesignDto
    {
        public List<RequestDto> Requests { get; set; }
        public string? ContractName { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
    }
}
