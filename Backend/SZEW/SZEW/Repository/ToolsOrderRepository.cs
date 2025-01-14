using SZEW.Data;
using SZEW.Interfaces;
using SZEW.Models;
using System.Collections.Generic;
using System.Linq;

namespace SZEW.Repository
{
    public class ToolsOrderRepository : IToolsOrderRepository
    {
        private readonly DataContext _context;

        public ToolsOrderRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<ToolsOrder> GetAllOrders()
        {
            return _context.ToolsOrders.OrderBy(t => t.Id).ToList();
        }

        public ToolsOrder GetOrderById(int id)
        {
            return _context.ToolsOrders.FirstOrDefault(t => t.Id == id);
        }

        public bool AddToolsOrder(ToolsOrder toolsOrder)
        {
            _context.ToolsOrders.Add(toolsOrder);
            return _context.SaveChanges() > 0;
        }

        public bool ToolsOrderExists(int id)
        {
            return _context.ToolsOrders.Any(t => t.Id == id);
        }
    }
}
