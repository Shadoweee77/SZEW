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

        public bool CreateVehicle(Vehicle vehicle)
        {
            _context.Add(vehicle);
            return Save();
        }

        public Vehicle GetVehicle(int id)
        {
            return _context.Vehicles.Where(p => p.Id == id).FirstOrDefault();
        }

        public Vehicle GetVehicle(string vin)
        {
            return _context.Vehicles.Where(p => p.VIN == vin).FirstOrDefault();
        }

        public ICollection<Vehicle> GetVehicles()
        {
            return _context.Vehicles.OrderBy(p => p.Id).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(p => p.Id == id);
        }

        public bool VehicleExists(string vin)
        {
            return _context.Vehicles.Any(v => v.VIN == vin);
        }
    }
}
