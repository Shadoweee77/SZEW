using SZEW.Models;

namespace SZEW.Interfaces
{
    public interface IWorkshopClientRepository
    {
        ICollection<WorkshopClient> GetClients();
        WorkshopClient GetClient(int id);
        ICollection<Vehicle> GetVehicles(int ownerId);
        ClientType GetClientType(int id);
        bool ClientExists(int id);
    }
}
