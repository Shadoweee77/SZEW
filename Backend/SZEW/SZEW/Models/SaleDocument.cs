namespace SZEW.Models
{
    public class SaleDocument
    {
        public required int Id { get; set; }
        public required DocumentType DocumentType { get; set; }
        public required DateTime IssueDate { get; set; } = DateTime.Now;
        public required bool IsPaid { get; set; }
        public required User DocumentIssuer { get; set; }
        public required int RelatedJobId { get; set; }
        public required WorkshopJob RelatedJob { get; set; }
    }
    public enum DocumentType
    {
        Paragon = 0,
        Faktura = 1
    }
}
