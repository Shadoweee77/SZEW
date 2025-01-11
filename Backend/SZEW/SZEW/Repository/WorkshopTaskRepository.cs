using SZEW.Data;
using SZEW.Interfaces;
using SZEW.Models;

namespace SZEW.Repository
{
    public class WorkshopTaskRepository : IWorkshopTaskRepository
    {
        private DataContext _context;

        public WorkshopTaskRepository(DataContext context)
        {
            this._context = context;
        }

        public bool WorkshopTaskExists(int id)
        {
            return _context.Tasks.Any(p => p.Id == id);
        }

        public ICollection<WorkshopTask> GetAllTasks()
        {
            return _context.Tasks.OrderBy(p => p.Id).ToList();
        }

        public WorkshopTask GetTaskById(int id)
        {
            return _context.Tasks.Where(p => p.Id == id).FirstOrDefault();
        }
    }
}
