namespace SZEW.Models
{
    public class Vehicle
    {
        public required int Id { get; set; }
        public string? VIN { get; set; }
        public required string Make { get; set; }
        public string? Model { get; set; }
        public DateTime? Year { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? Color { get; set; }
        public required WorkshopClient Owner { get; set; }
        public ICollection<WorkshopJob>? Jobs { get; set; }
    }
}