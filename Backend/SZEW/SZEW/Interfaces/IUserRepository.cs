using SZEW.Models;

namespace SZEW.Interfaces
{
    public interface IUserRepository
    {
        User GetByLogin(string login);
        User GetUserById(int id);
        bool UserExists(int id);
        ICollection<User> GetUsers();
        UserType GetUserType(int id);
    }
}
