using SZEW.Data;
using SZEW.Interfaces;
using SZEW.Models;

namespace SZEW.Repository
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly DataContext _context;

        public VehicleRepository(DataContext context)
        {
            this._context = context;
        }

        public ICollection<Vehicle> GetVehicles()
        {
            return _context.Vehicles.OrderBy(p => p.Id).ToList();
        }
    }
}
