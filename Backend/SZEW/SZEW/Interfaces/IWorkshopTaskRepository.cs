using SZEW.Models;

namespace SZEW.Interfaces
{
    public interface IWorkshopTaskRepository
    {
        ICollection<WorkshopTask> GetAllTasks();
        WorkshopTask GetTaskById(int id);
        bool WorkshopTaskExists(int id);
        //bool MarkComplete(int id, bool isComplete);
        //bool CreateTask();
        //bool DeleteTask();
        //bool UpdateTask();
    }
}
