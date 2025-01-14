using SZEW.Models;
using System.Collections.Generic;

namespace SZEW.Interfaces
{
    public interface IToolsRequestRepository
    {
        ICollection<ToolsRequest> GetAllRequests();
        ToolsRequest GetRequestById(int id);
        bool AddToolsRequest(ToolsRequest toolsRequest);
        bool ToolsRequestExists(int id);
        bool Save();
    }
}
