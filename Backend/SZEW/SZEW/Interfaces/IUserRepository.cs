using SZEW.Models;

namespace SZEW.Interfaces
{
    public interface IUserRepository
    {
        User GetByLogin(string login);
        User GetUserById(int id);
        bool UserExists(int id);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool ChangePassword(User user);
        ICollection<User> GetUsers();
        UserType GetUserType(int id);
        bool Save();
    }
}
