using SZEW.Data;
using SZEW.Interfaces;
using SZEW.Models;

namespace SZEW.Repository
{
    public class UserRepository : IUserRepository
    {
        public User GetUserByLogin(string login)
        {
            return _context.Users.FirstOrDefault(u => u.Login == login);
        }

        private DataContext _context;

        public UserRepository(DataContext context)
        {
            this._context = context;
        }
        public ICollection<User> GetAllUsers()
        {
            return _context.Users.OrderBy(p => p.Id).ToList();
        }

        public UserType GetUserType(int id)
        {
            return _context.Users.Where(p => p.Id == id).Select(p => p.UserType).FirstOrDefault();
        }

        public bool UserExists(int id)
        {
            return _context.Users.Any(p => p.Id == id);
        }

        public User GetUserById(int id)
        {
            return _context.Users.Where(p => p.Id == id).FirstOrDefault();
        }

        public bool CreateUser(User user)
        {
            _context.Add(user);
            return Save();
        }

        public bool UpdateUser(User user)
        {
            _context.Update(user);
            return Save();
        }

        public bool DeleteUser(User user)
        {
            _context.Remove(user);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool ChangePassword(User user)
        {
            _context.Update(user);
            return Save();
        }
    }
}
