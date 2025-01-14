namespace SZEW.DTO
{
    public class CreateVehicleDto
    {
        public string? VIN { get; set; }
        public required string Make { get; set; } //Use "Custom" for unknown
        public string? Model { get; set; }
        public int? Year { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? Color { get; set; }
    }
}
