namespace POS_System.Entities
{
    public class Category
    {
        public string Name { get; private set; }

        public Category(string name)
        {
            Name = name;
        }
    }
}
