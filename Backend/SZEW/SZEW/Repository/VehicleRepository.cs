using Microsoft.EntityFrameworkCore;
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

        public bool DeleteVehicle(Vehicle vehicle)
        {
            _context.Remove(vehicle);
            return Save();
        }

        public Vehicle GetVehicleById(int id)
        {
            return _context.Vehicles.Where(p => p.Id == id).Include(vehicle => vehicle.Owner).FirstOrDefault();
        }

        public Vehicle GetVehicleByVIN(string vin)
        {
            return _context.Vehicles.Where(p => p.VIN == vin).Include(vehicle => vehicle.Owner).FirstOrDefault();
        }

        public ICollection<Vehicle> GetAllVehicles()
        {
            return _context.Vehicles.Include(v => v.Owner).OrderBy(p => p.Id).ToList();

        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateVehicle(Vehicle vehicle)
        {
            _context.Update(vehicle);
            return Save();
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
