namespace SZEW.Models
{
    public class SparePart
    {
        public required int Id { get; set; }
        public required SparePartsOrder Order { get; set; }
        public required string Name { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
    }
}
