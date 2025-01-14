using SZEW.Models;

namespace SZEW.Interfaces
{
    public interface IVehicleRepository
    {
        ICollection<Vehicle> GetVehicles();
        Vehicle GetVehicle(int id);
        Vehicle GetVehicle(string vin);
        bool VehicleExists(int id);
        bool VehicleExists(string vin);
        bool CreateVehicle(Vehicle vehicle);
        bool UpdateVehicle(Vehicle vehicle);
        bool DeleteVehicle(Vehicle vehicle);
        bool Save();
    }
}
