using SZEW.Models;

namespace SZEW.Interfaces
{
    public interface ISparePartRepository
    {
        ICollection<SparePart> GetAllSpareParts();
        SparePart GetSparePartById(int id);
        bool SparePartExists(int id);
        bool CreateSparePart(SparePart sparePart); // Create a new spare part
        bool UpdateSparePart(SparePart sparePart); // Update an existing spare part
        bool DeleteSparePart(SparePart sparePart); // Delete a spare part
        bool Save(); // Save changes to the database
    }
}
