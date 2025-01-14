namespace SZEW.Models
{
    public class ToolsOrder
    {
        public required int Id { get; set; }
        public required User Orderer { get; set; }
        public required DateTime RegistrationDate { get; set; } = DateTime.Now;
        public ICollection<Tool>? Tools { get; set; }
    }
}
