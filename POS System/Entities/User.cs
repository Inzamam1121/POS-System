namespace POS_System.Entities
{
    public enum UserRole
    {
        Admin,
        Cashier
    }

    public class User
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public UserRole Role { get; set; }

        public User(string name, string email, string password, UserRole role)
        {
            Name = name;
            Email = email;
            Password = password;
            Role = role;
        }
    }
}
