namespace KoiPond.DTOs
{
    public class CreateRequestDesignDto
    {
        public UserDto User { get; set; }
        public string? RequestName { get; set; }
        public string? Description { get; set; }

        // Option to select either Design or Sample
        public bool IsDesignSelected { get; set; } // true: Design, false: Sample

        // Design and Sample DTOs (only one will be populated depending on the selection)
        public DesignDtoV1? Design { get; set; }

    }
}
