﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS_System.Data;
using POS_System.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace POS_System.Services
{
    public class SaleTransactionService
    {
        private readonly DBContextEntity _context;
        private Sale _currentSale;
        private readonly ILogger<SaleTransactionService> _logger;

        public SaleTransactionService(DBContextEntity context, ILogger<SaleTransactionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        private async Task<User> GetUserFromTokenAsync(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "id");

            if (userIdClaim == null)
            {
                _logger.LogError("User ID claim not found in token.");
                throw new Exception("User ID claim not found in token.");
            }

            var userId = int.Parse(userIdClaim.Value);
            User user = await _context.Users.FindAsync(userId);

            if (user != null)
            {
                _logger.LogInformation($"User Details: Id={user.UserID}, Name={user.Name}, Role={user.UserRole}");
            }
            else
            {
                _logger.LogError($"User with ID {userId} not found in the database.");
            }

            return user;
        }

        public async Task<bool> IsCashierAsync(string token)
        {
            User user = await GetUserFromTokenAsync(token);
            if (user != null && user.UserRole == UserRole.Cashier)
            {
                _logger.LogInformation($"User {user.Name} is a cashier.");
                return true;
            }
            _logger.LogWarning($"User is not a cashier or not found. Role: {user?.UserRole}");
            return false;
        }

        public async Task<bool> StartNewSaleAsync(string token)
        {
            var cashier = await GetUserFromTokenAsync(token);
            if (cashier == null || cashier.UserRole != UserRole.Cashier)
            {
                _logger.LogWarning("Invalid cashier.");
                return false;
            }

            _currentSale = new Sale
            {
                Cashier = cashier,
                CashierId = cashier.UserID,
                Date = DateTime.Now,
                Status = SaleStatus.Start
            };

            await _context.Sales.AddAsync(_currentSale);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddProductToSaleAsync(int productId, int quantity, string token)
        {
            var cashier = await GetUserFromTokenAsync(token);
            if (cashier == null || cashier.UserRole != UserRole.Cashier)
            {
                _logger.LogWarning("Invalid cashier.");
                return false;
            }

            if (_currentSale == null)
            {
                _logger.LogWarning("No sale in progress.");
                return false;
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                _logger.LogWarning("Product not found.");
                return false;
            }

            if (product.Quantity < quantity)
            {
                _logger.LogWarning("Insufficient quantity in inventory.");
                return false;
            }

            _currentSale.AddProduct(product, quantity);
            product.Quantity -= quantity;

            _context.Products.Update(product);

            // Handle ProductItem entity
            var productItem = _currentSale.Products.FirstOrDefault(pi => pi.ProductId == productId);
            if (productItem != null)
            {
                productItem.Quantity = quantity; // Update quantity if product already in sale
            }
            else
            {
                _currentSale.Products.Add(new ProductItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    SaleId = _currentSale.SaleId
                });
            }

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency exception occurred while adding product to sale.");
                // Handle concurrency exception (e.g., retry or notify user)
                return false;
            }
        }


        public async Task<bool> RemoveProductFromSaleAsync(int productId, int quantity, string token)
        {
            var cashier = await GetUserFromTokenAsync(token);
            if (_currentSale == null || _currentSale.Cashier != cashier)
            {
                _logger.LogWarning("No sale in progress or wrong cashier.");
                return false;
            }

            var productItem = _currentSale.Products.FirstOrDefault(pi => pi.ProductId == productId);
            if (productItem == null || productItem.Quantity < quantity)
            {
                _logger.LogWarning("Product not found in sale or insufficient quantity.");
                return false;
            }

            productItem.Quantity -= quantity;

            if (productItem.Quantity == 0)
            {
                _currentSale.Products.Remove(productItem);
            }

            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                product.Quantity += quantity;
                _context.Products.Update(product);
            }

            _context.ProductItems.Update(productItem);  
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CompleteSaleAsync(string token)
        {
            var cashier = await GetUserFromTokenAsync(token);
            if (_currentSale != null && _currentSale.Cashier == cashier)
            {
                _currentSale.Date = DateTime.Now;
                _currentSale.Status = SaleStatus.Complete;

                _context.Sales.Update(_currentSale);
                await _context.SaveChangesAsync();

                _currentSale = null;
                return true;
            }
            return false;
        }

        public Sale GetCurrentSale()
        {
            return _currentSale;
        }
    }
}
