using POS_System.Data;
using POS_System.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace POS_System.Services
{
    public class PurchaseTransactionService
    {
        private readonly DataContextEntity _context;
        private Purchase _currentPurchase;

        public PurchaseTransactionService(DataContextEntity context)
        {
            _context = context;
        }

        private async Task<User> GetUserFromTokenAsync(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = int.Parse(jwtToken.Claims.First(claim => claim.Type == "id").Value);
            return await _context.Users.FindAsync(userId);
        }

        public async Task<bool> StartNewPurchaseAsync(string token)
        {
            var user = await GetUserFromTokenAsync(token);
            if (user == null)
            {
                Console.WriteLine("Invalid user.");
                return false;
            }

            _currentPurchase = new Purchase();
            return true;
        }

        public async Task<bool> AddProductToPurchaseAsync(int productId, int quantity, string token)
        {
            var user = await GetUserFromTokenAsync(token);
            if (_currentPurchase == null)
            {
                if (!await StartNewPurchaseAsync(token))
                {
                    return false;
                }
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                Console.WriteLine("Product not found.");
                return false;
            }

            var productItem = _currentPurchase.Products.FirstOrDefault(pi => pi.ProductId == productId);
            if (productItem == null)
            {
                _currentPurchase.Products.Add(new ProductItem(product, quantity));
            }
            else
            {
                productItem.AddQuantity(quantity);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveProductFromPurchaseAsync(int productId, int quantity, string token)
        {
            var user = await GetUserFromTokenAsync(token);
            if (_currentPurchase == null)
            {
                Console.WriteLine("No purchase in progress.");
                return false;
            }

            var productItem = _currentPurchase.Products.FirstOrDefault(pi => pi.ProductId == productId);
            if (productItem == null || productItem.Quantity < quantity)
            {
                Console.WriteLine("Product not found in purchase or insufficient quantity.");
                return false;
            }

            productItem.Quantity -= quantity;
            if (productItem.Quantity == 0)
            {
                _currentPurchase.Products.Remove(productItem);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public Purchase GetCurrentPurchase()
        {
            return _currentPurchase;
        }
    }
}
