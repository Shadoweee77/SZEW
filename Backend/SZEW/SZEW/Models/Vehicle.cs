namespace SZEW.Models
{
    public class Vehicle
    {
        public required int Id { get; set; } //Some Vehicles don't have VIN
        public string? VIN { get; set; }
        public required string Make { get; set; } //Use "Custom" for unknown
        public string? Model { get; set; }
        public DateTime? Year { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? Color { get; set; }
        public required WorkshopClient Owner { get; set; }
    }
}