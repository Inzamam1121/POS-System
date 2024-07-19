namespace POS_System.Entities
{
    public class Product
    {
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; set; }
        public string Type { get; private set; }
        public Category Category { get; private set; }

        public Product(string name, decimal price, int quantity, string type, Category category)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
            Type = type;
            Category = category;
        }

        public void UpdateProduct(string name, decimal price, int quantity, string type, Category category)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
            Type = type;
            Category = category;
        }
    }
}
