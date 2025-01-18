using SZEW.Models;

namespace SZEW.Interfaces
{
    public interface IWorkshopTaskRepository
    {
        ICollection<WorkshopTask> GetAllTasks();
        WorkshopTask GetTaskById(int id);
        bool WorkshopTaskExists(int id);
        bool CreateWorkshopTask(WorkshopTask workshopTask);
        bool UpdateWorkshopTask(WorkshopTask workshopTask);
        bool DeleteWorkshopTask(WorkshopTask workshopTask);
        bool Save();
    }
}
