using Microsoft.EntityFrameworkCore;
using SZEW.Data;
using SZEW.Interfaces;
using SZEW.Models;

namespace SZEW.Repository
{
    public class SparePartRepository : ISparePartRepository
    {
        private readonly DataContext _context;

        public SparePartRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateSparePart(SparePart sparePart)
        {
            _context.Add(sparePart);
            return Save();
        }

        public bool DeleteSparePart(SparePart sparePart)
        {
            _context.Remove(sparePart);
            return Save();
        }

        public SparePart GetSparePartById(int id)
        {
            return _context.SpareParts.Where(s => s.Id == id).Include(s => s.Order).FirstOrDefault();
        }

        public ICollection<SparePart> GetAllSpareParts()
        {
            return _context.SpareParts.Include(s => s.Order).OrderBy(s => s.Id).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateSparePart(SparePart sparePart)
        {
            _context.Update(sparePart);
            return Save();
        }

        public bool SparePartExists(int id)
        {
            return _context.SpareParts.Any(s => s.Id == id);
        }
    }
}