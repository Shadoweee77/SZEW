using SZEW.Models;

namespace SZEW.DTO
{
    public class WorkshopJobDto
    {
        public required int Id { get; set; }
        public required Vehicle Vehicle { get; set; }
        public required bool Complete { get; set; }
        public required string Description { get; set; }
        public required DateTime AdmissionDate { get; set; }

        //public ICollection<WorkshopTask>? Tasks { get; set; }
    }
}
