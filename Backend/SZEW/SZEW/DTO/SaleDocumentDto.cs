using System;

namespace SZEW.DTO
{
    public class SaleDocumentDto
    {
        public int Id { get; set; }
        public int DocumentType { get; set; }
        public DateTime IssueDate { get; set; }
        public int DocumentIssuerId { get; set; }
        public int RelatedJobId { get; set; }
    }
}
