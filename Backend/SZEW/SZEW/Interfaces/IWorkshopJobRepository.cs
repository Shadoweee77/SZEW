using SZEW.Models;

namespace SZEW.Interfaces
{
    public interface IWorkshopJobRepository
    {
        ICollection<WorkshopJob> GetAllJobs();
        WorkshopJob GetJobById(int id);
        bool WorkshopJobExists(int id);
        bool MarkComplete(int id, bool isComplete);
        bool CreateWorkshopJob(WorkshopJob workshopJob);
        bool UpdateWorkshopJob(WorkshopJob workshopJob);
        bool DeleteWorkshopJob(WorkshopJob workshopJob);
        bool Save();
    }
}
