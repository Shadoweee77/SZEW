namespace SZEW.Models
{
    public class User
    {
        public required int Id { get; set; }
        public required string Login { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public  required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required UserType UserType { get; set; }
        public ICollection<ToolsOrder>? ToolsOrders { get; set; }
        public ICollection<SparePartsOrder>? SparePartsOrders { get; set; }
        public ICollection<SaleDocument>? IssuedSaleDocuments { get; set; }
        public ICollection<ToolsRequest>? ToolsRequestsRequested { get; set; }
        public ICollection<ToolsRequest>? ToolsRequestsVerified { get; set; }
    }

    public enum UserType
    {
        Mechanic = 0,
        Admin = 1
    }
}
