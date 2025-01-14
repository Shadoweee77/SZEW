using SZEW.Models;

namespace SZEW.Interfaces
{
    public interface ISparePartsOrderRepository
    {
        ICollection<SparePartsOrder> GetAllOrders();
        SparePartsOrder GetOrderById(int id);
        bool SparePartsOrderExists(int id);
        bool AddSparePartsOrder(SparePartsOrder sparePartsOrder);
        bool UpdateSparePartsOrder(SparePartsOrder sparePartsOrder);
        bool DeleteSparePartsOrder(int id);
    }
}
