namespace SZEW.Models
{
    public abstract class WorkshopClient
    {
        public required int Id { get; set; }
        public required string Email { get; set; }
        public required string Address { get; set; }
        public required string PhoneNumber { get; set; }
        public ICollection<Vehicle>? Vehicles { get; set; }
    }

    public enum ClientType
    {
        Individual = 0,
        Business = 1
    }
}
