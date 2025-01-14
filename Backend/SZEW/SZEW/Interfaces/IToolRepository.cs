using SZEW.Models;

namespace SZEW.Interfaces
{
    public interface IToolRepository
    {
        ICollection<Tool> GetTools();
        Tool GetToolById(int id);
        bool ToolExists(int id);
    }
}