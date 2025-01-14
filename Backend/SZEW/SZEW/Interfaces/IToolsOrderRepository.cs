using SZEW.Models;
using System.Collections.Generic;

namespace SZEW.Interfaces
{
    public interface IToolsOrderRepository
    {
        ICollection<ToolsOrder> GetAllOrders();
        ToolsOrder GetOrderById(int id);
        bool AddToolsOrder(ToolsOrder toolsOrder);
        bool ToolsOrderExists(int id);
    }
}
