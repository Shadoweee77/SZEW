using SZEW.Models;
using System.Collections.Generic;

namespace SZEW.Interfaces
{
    public interface IToolsRequestRepository
    {
        ICollection<ToolsRequest> GetAllRequests();
        ToolsRequest GetRequestById(int id);
        bool ToolsRequestExists(int id);
        bool CreateRequest(ToolsRequest request);
        bool VerifyRequest(ToolsRequest request);
        bool DeleteRequest(ToolsRequest request);
        bool Save();
    }
}
