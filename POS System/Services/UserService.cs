using POS_System.Data;
using POS_System.Entities;
using System.Linq;

namespace POS_System.Services
{
    public class UserService
    {
        public void RegisterUser(User user)
        {
            DataContext.Users.Add(user);
        }

        public User AuthenticateUser(string email, string password)
        {
            return DataContext.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public void SetUserRole(User user, UserRole role)
        {
            user.Role = role;
        }
    }
}
