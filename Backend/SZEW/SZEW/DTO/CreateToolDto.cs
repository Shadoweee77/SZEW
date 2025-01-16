namespace SZEW.DTO
{
    public class CreateToolDto
    {
        public required string Name { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        //public int OrderId { get; set; }
    }
}
