using SZEW.Models;

namespace SZEW.Interfaces
{
    public interface IUserRepository
    {
        User GetByLogin(string login);
        bool UserExists(int id);
        ICollection<User> GetUsers();
        UserType GetUserType(int id);
    }
}
