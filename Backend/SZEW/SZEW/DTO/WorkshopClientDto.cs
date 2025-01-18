using SZEW.Models;

namespace SZEW.DTO
{
    public class WorkshopClientDto
    {
        public required int Id { get; set; }
        public required string Email { get; set; }
        public required string Address { get; set; }
        public required string PhoneNumber { get; set; }
        public ICollection<int> VehicleIds { get; set; } = new List<int>();
        public required ClientType ClientType { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }

        public string? NIP { get; set; }
        public string? BusinessName { get; set; }
    }
}
