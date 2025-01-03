namespace SZEW.Models
{
    public class WorkshopTask
    {
        //UML - Usługa (wiele na jeden pojazd, w jednym zleceniu)
        public required int Id { get; set; }
        public required bool Complete { get; set; }
        public required string Description { get; set; }
        public required string Name { get; set; }
        public double Price { get; set; }
        public required User AssignedWorker { get; set; }
    }
}
