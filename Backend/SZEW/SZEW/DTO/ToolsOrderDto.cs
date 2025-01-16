using System;
using System.Collections.Generic;

namespace SZEW.DTO
{
    public class ToolsOrderDto
    {
        public int Id { get; set; }
        public int OrdererId { get; set; }
        public string OrdererName { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public ICollection<int> Tools { get; set; } = new List<int>();
    }
}
