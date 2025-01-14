using System;
using System.Collections.Generic;

namespace SZEW.DTO
{
    public class ToolsOrderDto
    {
        public int Id { get; set; }
        public int OrdererId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<int> ToolIds { get; set; }
    }
}
