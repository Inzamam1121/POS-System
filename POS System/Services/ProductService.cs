using POS_System.Data;
using POS_System.Entities;

namespace POS_System.Services
{
    public class ProductService
    {
        public void AddProduct(Product product, User admin)
        {
            if (admin.Role == UserRole.Admin)
            {
                DataContext.Products.Add(product);
            }
        }

        public void RemoveProduct(Product product, User admin)
        {
            if (admin.Role == UserRole.Admin)
            {
                DataContext.Products.Remove(product);
            }
        }

        public bool IsProductAvailable(Product product, int quantity)
        {
            return product.Quantity >= quantity;
        }

        public void UpdateProductQuantity(Product product, int quantity)
        {
            product.Quantity -= quantity;
        }
    }
}
