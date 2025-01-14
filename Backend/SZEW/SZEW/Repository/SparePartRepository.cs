using SZEW.Data;
using SZEW.Interfaces;
using SZEW.Models;
using System.Linq;

namespace SZEW.Repository
{
    public class SparePartRepository : ISparePartRepository
    {
        private DataContext _context;

        public SparePartRepository(DataContext context)
        {
            this._context = context;
        }

        public ICollection<SparePart> GetAllSpareParts()
        {
            return _context.SpareParts.OrderBy(s => s.Id).ToList();
        }

        public SparePart GetSparePartById(int id)
        {
            return _context.SpareParts.FirstOrDefault(s => s.Id == id);
        }

        public bool SparePartExists(int id)
        {
            return _context.SpareParts.Any(s => s.Id == id);
        }

        public bool AddSparePart(SparePart sparePart)
        {
            _context.SpareParts.Add(sparePart);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateSparePart(SparePart sparePart)
        {
            _context.SpareParts.Update(sparePart);
            return _context.SaveChanges() > 0;
        }

        public bool DeleteSparePart(int id)
        {
            var sparePart = _context.SpareParts.FirstOrDefault(s => s.Id == id);
            if (sparePart == null)
            {
                return false;
            }
            _context.SpareParts.Remove(sparePart);
            return _context.SaveChanges() > 0;
        }
    }
}
