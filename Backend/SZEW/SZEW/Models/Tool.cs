namespace SZEW.Models
{
    public class Tool
    {
        public required int Id { get; set; }
        public required ToolsOrder Order { get; set; }
        public required string Name{ get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
    }
}
