namespace SZEW.DTO
{
    public class UpdateVehicleDto
    {
        public string? VIN { get; set; }
        public required string Make { get; set; }
        public string? Model { get; set; }
        public DateTime? Year { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? Color { get; set; }
        public required int OwnerId { get; set; }
    }
}
