using Microsoft.EntityFrameworkCore;
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
         
        public async Task RegisterUserAsync(User user)
        { 
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
                throw new System.Exception("User with this email already exists.");

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> AuthenticateUserAsync(string emailOrUsername, string password)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => (u.Email == emailOrUsername || u.Username == emailOrUsername) && u.Password == password);
        }

        public async Task<bool> UpdateUserRoleAsync(string username, UserRole role)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return false;

            user.UserRole = role;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);
        }

    }
}
