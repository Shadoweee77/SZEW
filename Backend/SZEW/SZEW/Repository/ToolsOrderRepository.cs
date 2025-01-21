using Microsoft.EntityFrameworkCore;
using SZEW.Models;
using SZEW.Interfaces;
using SZEW.Data;

namespace SZEW.Repository
{
    public class ToolsOrderRepository : IToolsOrderRepository
    {
        private readonly DataContext _context;

        public ToolsOrderRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<ToolsOrder> GetAllToolsOrders()
        {
            return _context.ToolsOrders.OrderBy(o => o.Id).Include(ToolsOrder => ToolsOrder.Orderer).ToList();
        }

        public ToolsOrder GetToolsOrderById(int id)
        {
            return _context.ToolsOrders.Include(o => o.Tools).Include(ToolsOrder => ToolsOrder.Orderer).FirstOrDefault(o => o.Id == id);
        }

        public bool ToolsOrderExists(int id)
        {
            return _context.ToolsOrders.Any(o => o.Id == id);
        }

        public bool CreateToolsOrder(ToolsOrder toolsOrder)
        {
            _context.Add(toolsOrder);
            return Save();
        }

        public bool UpdateToolsOrder(ToolsOrder toolsOrder)
        {
            _context.Update(toolsOrder);
            return Save();
        }

        public bool DeleteToolsOrder(ToolsOrder toolsOrder)
        {
            _context.Remove(toolsOrder);
            return Save();
        }

        public bool Save()
        {
            var changes = _context.SaveChanges();
            return changes > 0;
        }
    }
}
