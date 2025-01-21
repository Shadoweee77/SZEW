using SZEW.Models;

namespace SZEW.Interfaces
{
    public interface IToolRepository
    {
        ICollection<Tool> GetAllTools();
        Tool GetToolById(int id);
        bool ToolExists(int id);
        bool CreateTool(Tool tool);
        bool UpdateTool(Tool tool);
        bool DeleteTool(Tool tool);
        bool Save();
    }
}
