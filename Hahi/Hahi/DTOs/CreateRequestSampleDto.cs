namespace Hahi.DTOs
{
    public class CreateRequestSampleDto
    {
        public UserDto User { get; set; }
        public string? RequestName { get; set; }
        public string? Description { get; set; }
        public bool IsSampleSelected { get; set; } // true: Sample, false: Design
        public SampleDtoV1? Sample { get; set; }
    }
}
