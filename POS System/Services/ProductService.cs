using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using POS_System.Data;
using POS_System.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace POS_System.Services
{
    public class ProductService
    {
        private readonly DBContextEntity _context;

        public ProductService(DBContextEntity context)
        {
            _context = context;
        }

        public async Task<bool> AddProductAsync(Product product, string token)
        {
            var admin = await GetUserFromTokenAsync(token);
            if (admin != null && admin.UserRole == UserRole.Admin)
            {
                var categoryExists = await _context.Categories.AnyAsync(c => c.Id == product.CategoryId);
                if (!categoryExists)
                {
                    throw new Exception("Category not found.");
                }

                product.Category = await _context.Categories.FindAsync(product.CategoryId);

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<bool> UpdateProductAsync(Product product, string token)
        {
            var admin = await GetUserFromTokenAsync(token);
            if (admin != null && admin.UserRole == UserRole.Admin)
            {
                var existingProduct = await _context.Products.FindAsync(product.ProductId);

                if (existingProduct == null)
                {
                    throw new Exception("Product not found.");
                }

                var categoryExists = await _context.Categories.AnyAsync(c => c.Id == product.CategoryId);
                if (!categoryExists)
                {
                    throw new Exception("Category not found.");
                }

                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Quantity = product.Quantity;
                existingProduct.Type = product.Type;
                existingProduct.CategoryId = product.CategoryId;

                _context.Products.Update(existingProduct);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<bool> RemoveProductAsync(int productId, string token)
        {
            var admin = await GetUserFromTokenAsync(token);
            if (admin != null && admin.UserRole == UserRole.Admin)
            {
                var product = await _context.Products.FindAsync(productId);
                if (product != null)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> DeleteProductAsync(int productId, string token)
        {
            return await RemoveProductAsync(productId, token); 
        }

        public async Task<bool> UpdateProductQuantityAsync(int productId, int quantity, string token)
        {
            var admin = await GetUserFromTokenAsync(token);
            if (admin != null && admin.UserRole == UserRole.Admin)
            {
                var product = await _context.Products.FindAsync(productId);
                if (product != null)
                {
                    product.Quantity = quantity;
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task<User> GetUserFromTokenAsync(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = int.Parse(jwtToken.Claims.First(claim => claim.Type == "id").Value);
            return await _context.Users.FindAsync(userId);
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p=>p.Category)
                .ToListAsync();
        }

        public async Task<Product> GetProductAsync(int productId)
        {
            return await _context.Products
                .Include (p=>p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }
    }
}
