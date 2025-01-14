using SZEW.Models;

namespace SZEW.Interfaces
{
    public interface ISparePartRepository
    {
        ICollection<SparePart> GetAllSpareParts();
        SparePart GetSparePartById(int id);
        bool SparePartExists(int id);
    }
}
