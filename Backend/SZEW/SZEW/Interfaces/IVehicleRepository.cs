using SZEW.Models;

namespace SZEW.Interfaces
{
    public interface IVehicleRepository
    {
        ICollection<Vehicle> GetAllVehicles();
        Vehicle GetVehicleById(int id);
        Vehicle GetVehicleByVIN(string vin);
        bool VehicleExists(int id);
        bool VehicleExists(string vin);
        bool CreateVehicle(Vehicle vehicle);
        bool UpdateVehicle(Vehicle vehicle);
        bool DeleteVehicle(Vehicle vehicle);
        bool Save();
    }
}
