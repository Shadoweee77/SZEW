using System.ComponentModel.DataAnnotations.Schema;

namespace SZEW.Models
{
    public class ToolsRequest
    {
        public required int Id { get; set; }
        [ForeignKey("Requester")]
        public required int RequesterId { get; set; }
        public required User Requester { get; set; }

        [ForeignKey("Verifier")]
        public int? VerifierId { get; set; }  
        public User? Verifier { get; set; }
        public bool Verified { get; set; }
        public required string Description { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public required DateTime RequestDate { get; set; }
    }
}
