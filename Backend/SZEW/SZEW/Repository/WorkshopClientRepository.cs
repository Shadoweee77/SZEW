using Microsoft.EntityFrameworkCore;
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

        public bool CreateWorkshopClient(WorkshopClient workshopClient)
        {
            _context.Add(workshopClient);
            return Save();
        }

        public bool DeleteWorkshopClient(WorkshopClient workshopClient)
        {
            _context.Remove(workshopClient);
            return Save();
        }

        public WorkshopClient GetClient(int id)
        {
            return _context.Clients.Where(p => p.Id == id).Include(client => client.Vehicles).FirstOrDefault();
        }

        public ICollection<WorkshopClient> GetClients()
        {
            return _context.Clients.Include(c => c.Vehicles).ToList().OrderBy(p => p.Id).ToList();
        }

        public ClientType GetClientType(int id)
        {
            return _context.Clients.Where(p =>p.Id == id).Select(p => p.ClientType).FirstOrDefault();
        }

        public ICollection<Vehicle> GetVehicles(int ownerId)
        {
            return _context.Vehicles.Where(p => p.Owner.Id == ownerId).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateWorkshopClient(WorkshopClient workshopClient)
        {
            _context.Update(workshopClient);
            return Save();
        }
    }
}
