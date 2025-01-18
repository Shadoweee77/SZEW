namespace SZEW.DTO
{
    public class CreateVehicleDto
    {
        public string? VIN { get; set; }
        public required string Make { get; set; }
        public string? Model { get; set; }
        public DateTime? Year { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? Color { get; set; }
    }
}
