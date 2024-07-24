using POS_System.Entities;
using System.Collections.Generic;

namespace POS_System.Data
{
    public static class DBContext
    {
        public static List<User> Users { get; set; } = new List<User>();
        public static List<Product> Products { get; set; } = new List<Product>();
        public static List<Category> Categories { get; set; } = new List<Category>();
        public static List<Sale> Sales { get; set; } = new List<Sale>();
    }
}
