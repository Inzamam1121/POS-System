namespace POS_System.DTOs
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public int CategoryId { get; set; }

        public ProductDTO() { }

        public ProductDTO(int productId, string name, decimal price, int quantity, string type, int categoryId)
        {
            ProductId = productId;
            Name = name;
            Price = price;
            Quantity = quantity;
            Type = type;
            CategoryId = categoryId;
        }
    }
}


