using SZEW.Data;
using SZEW.Interfaces;
using SZEW.Models;
using System.Collections.Generic;
using System.Linq;

namespace SZEW.Repository
{
    public class ToolsRequestRepository : IToolsRequestRepository
    {
        private readonly DataContext _context;

        public ToolsRequestRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<ToolsRequest> GetAllRequests()
        {
            return _context.ToolsRequests.OrderBy(r => r.Id).ToList();
        }

        public ToolsRequest GetRequestById(int id)
        {
            return _context.ToolsRequests.FirstOrDefault(r => r.Id == id);
        }

        public bool AddToolsRequest(ToolsRequest toolsRequest)
        {
            _context.ToolsRequests.Add(toolsRequest);
            return Save();
        }

        public bool ToolsRequestExists(int id)
        {
            return _context.ToolsRequests.Any(r => r.Id == id);
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
