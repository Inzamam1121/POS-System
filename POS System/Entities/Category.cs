namespace POS_System.Entities
{
    public class Category
    {
        public int Id { get; set; } // Primary key
        public string Name { get; set; }

        // Parameterless constructor for EF
        public Category() { }

        // Constructor for convenience
        public Category(string name)
        {
            Name = name;
        }
    }
}
