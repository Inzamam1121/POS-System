using POS_System.Entities;

namespace POS_System.DTOs
{
    public class UserDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public int UserID { get; set; }
        public UserRole UserRole { get; set; }

        public UserDTO() { }

        public UserDTO(string name, string email, string username, int userId, UserRole userRole)
        {
            Name = name;
            Email = email;
            Username = username;
            UserID = userId;
            UserRole = userRole;
        }
    }
}
