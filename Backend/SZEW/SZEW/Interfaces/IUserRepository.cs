using SZEW.Models;

namespace SZEW.Interfaces
{
    public interface IUserRepository
    {
        bool UserExists(int id);
        ICollection<User> GetUsers();
        UserType GetUserType(int id);
    }
}
