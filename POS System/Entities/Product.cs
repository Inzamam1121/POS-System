namespace POS_System.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        // Parameterless constructor for EF
        public Product() { }

        // Constructor for convenience
        public Product(string name, decimal price, int quantity, string type, Category category)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
            Type = type;
            Category = category;
        }

        public Product(string name, decimal price, int quantity, string type)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
            Type = type;
        }

        public Product(string name, decimal price, int quantity, string type, int categoryId)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
            Type = type;
            CategoryId = categoryId;
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
