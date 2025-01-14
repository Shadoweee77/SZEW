using SZEW.Data;
using SZEW.Interfaces;
using SZEW.Models;
using System.Linq;

namespace SZEW.Repository
{
    public class ToolRepository : IToolRepository
    {
        private readonly DataContext _context;

        public ToolRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Tool> GetTools()
        {
            return _context.Tools.OrderBy(t => t.Id).ToList();
        }

        public Tool GetToolById(int id)
        {
            return _context.Tools.FirstOrDefault(t => t.Id == id);
        }

        public bool ToolExists(int id)
        {
            return _context.Tools.Any(t => t.Id == id);
        }
    }
}