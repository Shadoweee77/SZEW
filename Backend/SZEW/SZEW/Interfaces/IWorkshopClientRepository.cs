using SZEW.Models;

namespace SZEW.Interfaces
{
    public interface IWorkshopClientRepository
    {
        ICollection<WorkshopClient> GetAllWorkshopClients();
        WorkshopClient GetWorkshopClientById(int id);
        ICollection<Vehicle> GetWorkshopClientsVehicles(int ownerId);
        ClientType GetWorkshopClientType(int id);
        bool WorkshopClientExists(int id);
        bool CreateWorkshopClient(WorkshopClient workshopClient);
        bool UpdateWorkshopClient(WorkshopClient workshopClient);
        bool DeleteWorkshopClient(WorkshopClient workshopClient);
        bool Save();
    }
}
