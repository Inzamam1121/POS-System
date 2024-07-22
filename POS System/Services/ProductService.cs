using POS_System.Data;
using POS_System.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace POS_System.Services
{
    public class ProductService
    {
        private readonly DataContextEntity _context;

        public ProductService(DataContextEntity context)
        {
            _context = context;
        }

        public async Task<bool> AddProduct(Product product, User admin)
        {
            if (admin.Role == UserRole.Admin)
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task RemoveProduct(Product product, User admin)
        {
            if (admin.Role == UserRole.Admin)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public bool IsProductAvailable(Product product, int quantity)
        {
            return product.Quantity >= quantity;
        }

        public void UpdateProductQuantity(Product product, int quantity)
        {
            product.Quantity -= quantity;
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public async Task<bool> UpdateProduct(Product product, User admin)
        {
            if (admin.Role == UserRole.Admin)
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
