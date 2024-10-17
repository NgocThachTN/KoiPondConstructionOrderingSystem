namespace Hahi.DTOs
{
    public class CreateContractDesignDto
    {
        public List<RequestDesignDto> Requests { get; set; } // Use a specific DTO for design
        public int ContractId { get; set; }
        public string? ContractName { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
    }

    public class RequestDesignDto
    {
        public List<UserDto> Users { get; set; }
        public List<DesignDtoV1> Designs { get; set; }// Only include Design here
        public int RequestId { get; set; }
        public string? RequestName { get; set; }
        public string? Description { get; set; }
    }
}