namespace POS_System.Entities
{
    public enum UserRole
    {
        Admin,
        Cashier
    }

    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string AccessToken { get; set; }
        public int UserID { get; set; }
        public UserRole UserRole { get; set; }
         
        public User() { }
         
        public User(string name, string email, string password, string username, string accessToken, int userId, UserRole userRole)
        {
            Name = name;
            Email = email;
            Password = password;
            Username = username;
            AccessToken = accessToken;
            UserID = userId;
            UserRole = userRole;
        }
    }
}
