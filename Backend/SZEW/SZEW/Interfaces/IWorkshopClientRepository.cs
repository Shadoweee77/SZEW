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
        bool CreateWorkshopClient(WorkshopClient workshopClient);
        bool UpdateWorkshopClient(WorkshopClient workshopClient);
        bool DeleteWorkshopClient(WorkshopClient workshopClient);
        bool Save();
    }
}
