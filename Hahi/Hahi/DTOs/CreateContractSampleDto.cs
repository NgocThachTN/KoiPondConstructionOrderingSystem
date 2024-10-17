namespace Hahi.DTOs
{
    public class CreateContractSampleDto
    {
        public List<RequestSampleDto> Requests { get; set; } // Use a specific DTO for sample
        public int ContractId { get; set; }
        public string? ContractName { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
    }

    public class RequestSampleDto
    {
        public List<UserDto> Users { get; set; }
        public List<SampleDtoV1> Samples { get; set; } // Only include Sample here
        public string? RequestName { get; set; }
        public string? Description { get; set; }
    }
}
