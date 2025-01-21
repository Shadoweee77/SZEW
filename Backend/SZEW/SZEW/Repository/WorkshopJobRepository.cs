using SZEW.Data;
using SZEW.Interfaces;
using SZEW.Models;

namespace SZEW.Repository
{
    public class WorkshopJobRepository : IWorkshopJobRepository
    {
        private DataContext _context;

        public WorkshopJobRepository(DataContext context)
        {
            this._context = context;
        }

        public bool CreateWorkshopJob(WorkshopJob workshopJob)
        {
            _context.Add(workshopJob);
            return Save();
        }

        public bool DeleteWorkshopJob(WorkshopJob workshopJob)
        {
            _context.Remove(workshopJob);
            return Save();
        }

        public ICollection<WorkshopJob> GetAllWorkshopJobs()
        {
            return _context.Jobs.OrderBy(p => p.Id).ToList();
        }

        public WorkshopJob GetWorkshopJobById(int id)
        {
            return _context.Jobs.Where(p => p.Id == id).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateWorkshopJob(WorkshopJob workshopJob)
        {
            _context.Update(workshopJob);
            return Save();
        }

        public bool WorkshopJobExists(int id)
        {
            return _context.Jobs.Any(p => p.Id == id);
        }
    }
}
