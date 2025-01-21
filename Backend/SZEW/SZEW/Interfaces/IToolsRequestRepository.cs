using SZEW.Models;

namespace SZEW.Interfaces
{
    public interface IToolsRequestRepository
    {
        ICollection<ToolsRequest> GetAllToolsRequests();
        ToolsRequest GetToolsRequestById(int id);
        bool ToolsRequestExists(int id);
        bool CreateToolsRequest(ToolsRequest request);
        bool UpdateToolsRequest(ToolsRequest request);
        bool DeleteToolsRequest(ToolsRequest request);
        bool Save();
    }
}
