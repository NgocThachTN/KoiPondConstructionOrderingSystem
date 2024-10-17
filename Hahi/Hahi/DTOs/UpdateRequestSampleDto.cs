namespace Hahi.DTOs
{
    public class UpdateRequestSampleDto
    {
        public UserDto User { get; set; }
        public int RequestId { get; set; }
        public string? RequestName { get; set; }
        public string? Description { get; set; }
        public SampleDtoV1? Sample { get; set; }
    }
}
