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
        private readonly DataContextEntity _context;

        public ProductService(DataContextEntity context)
        {
            _context = context;
        }

        public async Task<bool> AddProductAsync(Product product, string token)
        {
            var admin = await GetUserFromTokenAsync(token);
            if (admin != null && admin.UserRole == UserRole.Admin)
            {
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
                _context.Products.Update(product);
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

        public async Task<Product> GetProductAsync(int productId)
        {
            return await _context.Products.FindAsync(productId);
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

        private async Task<User> GetUserFromTokenAsync(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = int.Parse(jwtToken.Claims.First(claim => claim.Type == "id").Value);
            return await _context.Users.FindAsync(userId);
        }
    }
}
