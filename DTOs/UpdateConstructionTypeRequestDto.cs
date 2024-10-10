namespace KoiPond.DTOs
{
    public class UpdateConstructionTypeRequestDto
    {
        public string? ConstructionName { get; set; }

        public List<DesignDto> Designs { get; set; } // Ensure this is present
        public List<SampleDto> Samples { get; set; } // Ensure this is present
    }
}
