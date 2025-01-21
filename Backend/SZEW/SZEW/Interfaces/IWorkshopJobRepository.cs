using SZEW.Models;

namespace SZEW.Interfaces
{
    public interface IWorkshopJobRepository
    {
        ICollection<WorkshopJob> GetAllWorkshopJobs();
        WorkshopJob GetWorkshopJobById(int id);
        bool WorkshopJobExists(int id);
        bool CreateWorkshopJob(WorkshopJob workshopJob);
        bool UpdateWorkshopJob(WorkshopJob workshopJob);
        bool DeleteWorkshopJob(WorkshopJob workshopJob);
        bool Save();
    }
}
