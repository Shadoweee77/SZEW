using SZEW.Models;

namespace SZEW.DTO
{
    public class CreateWorkshopJobDto
    {
        public required bool Complete { get; set; }
        public required string Description { get; set; }
    }
}
