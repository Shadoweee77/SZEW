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

        public ICollection<WorkshopTask> GetAllWorkshopTasks()
        {
            return _context.Tasks.OrderBy(p => p.Id).ToList();
        }

        public WorkshopTask GetWorkshopTaskById(int id)
        {
            return _context.Tasks.Where(p => p.Id == id).FirstOrDefault();
        }

        public bool CreateWorkshopTask(WorkshopTask workshopTask)
        {
            _context.Add(workshopTask);
            return Save();
        }

        public bool UpdateWorkshopTask(WorkshopTask workshopTask)
        {
            _context.Update(workshopTask);
            return Save();
        }

        public bool DeleteWorkshopTask(WorkshopTask workshopTask)
        {
            _context.Remove(workshopTask);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
