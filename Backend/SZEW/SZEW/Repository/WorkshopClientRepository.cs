using SZEW.Data;
using SZEW.Interfaces;
using SZEW.Models;

namespace SZEW.Repository
{
    public class WorkshopClientRepository : IWorkshopClientRepository
    {
        private DataContext _context;
        public WorkshopClientRepository(DataContext context)
        {
            this._context = context;
        }

        public bool ClientExists(int id)
        {
            return _context.Clients.Any(p => p.Id == id);
        }

        public WorkshopClient GetClient(int id)
        {
            return _context.Clients.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<WorkshopClient> GetClients()
        {
            return _context.Clients.OrderBy(p => p.Id).ToList();
        }

        public ClientType GetClientType(int id)
        {
            return _context.Clients.Where(p =>p.Id == id).Select(p => p.ClientType).FirstOrDefault();
        }

        public ICollection<Vehicle> GetVehicles(int ownerId)
        {
            return _context.Vehicles.Where(p => p.Owner.Id == ownerId).ToList();
        }
    }
}
