using SZEW.Models;

namespace SZEW.Interfaces
{
    public interface IVehicleRepository
    {
        ICollection<Vehicle> GetVehicles();
    }
}
