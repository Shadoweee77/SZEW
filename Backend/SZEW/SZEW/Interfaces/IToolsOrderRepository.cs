using SZEW.Models;

namespace SZEW.Interfaces
{
    public interface IToolsOrderRepository
    {
        ICollection<ToolsOrder> GetAllOrders();
        ToolsOrder GetOrderById(int id);
        bool ToolsOrderExists(int id);
        bool CreateToolsOrder(ToolsOrder toolsOrder);
        bool UpdateToolsOrder(ToolsOrder toolsOrder);
        bool DeleteToolsOrder(ToolsOrder toolsOrder);
        bool Save();
    }
}
