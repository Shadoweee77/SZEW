using SZEW.Models;

namespace SZEW.DTO
{
    public class WorkshopJobDto
    {
        public required int Id { get; set; }
        
        public required bool IsComplete { get; set; }
        public required string Description { get; set; }
        public required DateTime AdmissionDate { get; set; } = DateTime.Now;
        //public required Vehicle Vehicle { get; set; }

        //public ICollection<WorkshopTask>? Tasks { get; set; }
    }
}
