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

        public ICollection<WorkshopJob> GetAllJobs()
        {
            return _context.Jobs.OrderBy(p => p.Id).ToList();
        }

        public WorkshopJob GetJobById(int id)
        {
            return _context.Jobs.Where(p => p.Id == id).FirstOrDefault();
        }

        public bool MarkComplete(int id, bool isComplete)
        {
            return _context.Jobs.Where(p => p.Id == id).FirstOrDefault().Complete = isComplete;
        }

        public bool WorkshopJobExists(int id)
        {
            throw new NotImplementedException();
        }
    }
}
