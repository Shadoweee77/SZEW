using SZEW.Data;
using SZEW.Interfaces;
using SZEW.Models;

namespace SZEW.Repository
{
    public class UserRepository : IUserRepository
    {
        private DataContext _context;

        public UserRepository(DataContext context)
        {
            this._context = context;
        }
        public ICollection<User> GetUsers()
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
    }
}
