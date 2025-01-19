using Microsoft.EntityFrameworkCore;
using SZEW.Data;
using SZEW.Interfaces;
using SZEW.Models;

namespace SZEW.Repository
{
    public class SparePartsOrderRepository : ISparePartsOrderRepository
    {
        private readonly DataContext _context;

        public SparePartsOrderRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<SparePartsOrder> GetAllOrders()
        {
            return _context.SparePartsOrders.Include(SparePartsOrder => SparePartsOrder.Orderer).OrderBy(o => o.Id).ToList();
        }

        public SparePartsOrder GetOrderById(int id)
        {
            return _context.SparePartsOrders.Include(SparePartsOrder => SparePartsOrder.Orderer).Where(o => o.Id == id).FirstOrDefault();
        }

        public bool SparePartsOrderExists(int id)
        {
            return _context.SparePartsOrders.Any(o => o.Id == id);
        }

        public bool AddSparePartsOrder(SparePartsOrder sparePartsOrder)
        {
            _context.SparePartsOrders.Add(sparePartsOrder);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateSparePartsOrder(SparePartsOrder sparePartsOrder)
        {
            _context.SparePartsOrders.Update(sparePartsOrder);
            return _context.SaveChanges() > 0;
        }

        public bool DeleteSparePartsOrder(int id)
        {
            var order = _context.SparePartsOrders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return false;
            }
            _context.SparePartsOrders.Remove(order);
            return _context.SaveChanges() > 0;
        }
    }
}
