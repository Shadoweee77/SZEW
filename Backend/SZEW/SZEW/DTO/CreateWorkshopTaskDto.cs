namespace SZEW.DTO
{
    public class CreateWorkshopTaskDto
    {
        public required bool Complete { get; set; }
        public required string Description { get; set; }
        public required string Name { get; set; }
        public double Price { get; set; } 
    }
}
