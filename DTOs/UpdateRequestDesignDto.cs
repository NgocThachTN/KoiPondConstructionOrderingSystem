﻿namespace KoiPond.DTOs
{
    public class UpdateRequestDesignDto
    {
        public UserDto User { get; set; }
        public string? RequestName { get; set; }
        public string? Description { get; set; }
        public DesignDtoV1? Design { get; set; }
    }
}
