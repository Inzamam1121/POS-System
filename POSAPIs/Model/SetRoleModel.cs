using POS_System.Entities;

namespace POSAPIs.Model
{
    public class SetRoleModel
    {
        public string Username { get; set; }
        public UserRole Role { get; set; }
    }
}
