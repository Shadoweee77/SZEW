using SZEW.Data;
using SZEW.Interfaces;
using SZEW.Models;

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

        public bool ToolsRequestExists(int id)
        {
            return _context.ToolsRequests.Any(r => r.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool CreateRequest(ToolsRequest request)
        {
            _context.Add(request);
            return Save();
        }

        public bool DeleteRequest(ToolsRequest request)
        {
            _context.Remove(request);
            return Save();
        }

        public bool VerifyRequest(ToolsRequest request)
        {
            _context.ToolsRequests.Update(request);
            return Save();
        }
    }
}
