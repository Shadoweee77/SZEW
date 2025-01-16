using SZEW.Models;

namespace SZEW.DTO
{
    public class WorkshopTaskDto
    {
        public required int Id { get; set; }
        public required bool Complete { get; set; }
        public required string Description { get; set; }
        public required string Name { get; set; }
        public double Price { get; set; }
    }
}
