using Microsoft.EntityFrameworkCore;
using SZEW.Models;
using SZEW.Interfaces;
using System.Collections.Generic;
using System.Linq;
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

        // Get all ToolsOrders
        public ICollection<ToolsOrder> GetAllOrders()
        {
            return _context.ToolsOrders.OrderBy(o => o.Id).ToList();
        }

        // Get a specific ToolsOrder by ID
        public ToolsOrder GetOrderById(int id)
        {
            return _context.ToolsOrders
                           .Include(o => o.Tools) // Assuming ToolsOrder has a navigation property to Tool
                           .FirstOrDefault(o => o.Id == id);
        }

        // Check if a ToolsOrder exists
        public bool ToolsOrderExists(int id)
        {
            return _context.ToolsOrders.Any(o => o.Id == id);
        }

        // Create a new ToolsOrder
        public bool CreateToolsOrder(ToolsOrder toolsOrder)
        {
            _context.Add(toolsOrder);
            return Save();
        }

        // Update an existing ToolsOrder
        public bool UpdateToolsOrder(ToolsOrder toolsOrder)
        {
            _context.Update(toolsOrder);
            return Save();
        }

        // Delete a ToolsOrder
        public bool DeleteToolsOrder(ToolsOrder toolsOrder)
        {
            _context.Remove(toolsOrder);
            return Save();
        }

        // Save changes to the database
        public bool Save()
        {
            var changes = _context.SaveChanges();
            return changes > 0;
        }
    }
}
