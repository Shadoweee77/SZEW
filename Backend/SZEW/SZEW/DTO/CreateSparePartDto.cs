namespace SZEW.DTO
{
    public class CreateSparePartDto
    {
        public required string Name { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
    }
}
