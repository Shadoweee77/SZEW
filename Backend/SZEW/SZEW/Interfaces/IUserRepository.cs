using SZEW.Models;

namespace SZEW.Interfaces
{
    public interface IUserRepository
    {
        User GetUserByLogin(string login);
        User GetUserById(int id);
        bool UserExists(int id);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool ChangePassword(User user);
        ICollection<User> GetAllUsers();
        UserType GetUserType(int id);
        bool Save();
    }
}
