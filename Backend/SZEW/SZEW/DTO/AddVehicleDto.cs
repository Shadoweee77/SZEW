namespace SZEW.DTO
{
    public class AddVehicleDto
    {
        public string? VIN { get; set; }
        public required string Make { get; set; } //Use "Custom" for unknown
        public string? Model { get; set; }
        public DateTime? Year { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? Color { get; set; }
    }
}
