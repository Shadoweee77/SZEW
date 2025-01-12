namespace SZEW.Models
{
    public class ToolsOrder
    {
        public required int Id { get; set; }
        public required User Orderer { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public ICollection<Tool>? Tools { get; set; }
    }
}
