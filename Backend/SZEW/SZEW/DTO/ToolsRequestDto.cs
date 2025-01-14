using System;

namespace SZEW.DTO
{
    public class ToolsRequestDto
    {
        public int Id { get; set; }
        public int RequesterId { get; set; }
        public int? VerifierId { get; set; }
        public bool Verified { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
