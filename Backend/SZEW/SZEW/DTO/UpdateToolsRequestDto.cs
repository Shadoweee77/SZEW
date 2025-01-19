namespace SZEW.DTO
{
    public class UpdateToolsRequestDto
    {
        public bool Accepted { get; set; }
        public required string Description { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
    }
}
