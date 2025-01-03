using SZEW.Models;

namespace SZEW.Interfaces
{
    public interface IWorkshopTaskRepository
    {
        ICollection<WorkshopTask> GetAllTasks();
        WorkshopTask GetById(int id);
        bool WorkshopTaskExists(int id);
    }
}
