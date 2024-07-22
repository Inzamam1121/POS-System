namespace POS_System.Entities
{
    public enum UserRole
    {
        Admin,
        Cashier
    }

    public class User
    {
        public int Id { get; set; } // Primary key
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }

        // Parameterless constructor for EF
        public User() { }

        // Constructor for convenience
        public User(string name, string email, string password, UserRole role)
        {
            Name = name;
            Email = email;
            Password = password;
            Role = role;
        }
    }
}
