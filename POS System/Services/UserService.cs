using POS_System.Data;
using POS_System.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace POS_System.Services
{
    public class UserService
    {
        private readonly DataContextEntity _context;

        public UserService(DataContextEntity context)
        {
            _context = context;
        }

        public async Task RegisterUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public User AuthenticateUser(string email, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public async Task SetUserRole(User user, UserRole role)
        {
            user.Role = role;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
