using Microsoft.EntityFrameworkCore;
using SZEW.Data;
using SZEW.Interfaces;
using SZEW.Models;

namespace SZEW.Repository
{
    public class ToolRepository : IToolRepository
    {
        private readonly DataContext _context;

        public ToolRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateTool(Tool tool)
        {
            _context.Add(tool);
            return Save();
        }

        public bool DeleteTool(Tool tool)
        {
            _context.Remove(tool);
            return Save();
        }

        public Tool GetToolById(int id)
        {
            return _context.Tools
                .Where(t => t.Id == id)
                .Include(t => t.Order)
                .FirstOrDefault();
        }

        public ICollection<Tool> GetTools()
        {
            return _context.Tools
                .Include(t => t.Order)
                .OrderBy(t => t.Id)
                .ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateTool(Tool tool)
        {
            _context.Update(tool);
            return Save();
        }

        public bool ToolExists(int id)
        {
            return _context.Tools.Any(t => t.Id == id);
        }
    }
}
