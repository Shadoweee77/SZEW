using SZEW.Models;

namespace SZEW.DTO
{
    public class WorkshopClientDto
    {
        public required int Id { get; set; }
        public required string Email { get; set; }
        public required string Address { get; set; }
        public required string PhoneNumber { get; set; }
        public ICollection<Vehicle>? Vehicles { get; set; }
        public required ClientType ClientType { get; set; }
    }
}
