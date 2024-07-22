using POS_System.Data;
using POS_System.Entities;
using System;
using System.Threading.Tasks;

namespace POS_System.Services
{
    public class SaleTransactionService
    {
        private readonly DataContextEntity _context;
        private Sale _currentSale;

        public SaleTransactionService(DataContextEntity context)
        {
            _context = context;
        }

        public void StartNewSale(User cashier)
        {
            _currentSale = new Sale(cashier);
        }

        public async Task<bool> AddProductToSaleAsync(Product product, int quantity, User cashier)
        {
            if (_currentSale == null || _currentSale.Cashier != cashier)
            {
                StartNewSale(cashier);
            }

            if (product.Quantity >= quantity)
            {
                _currentSale.AddProductToSale(product, quantity);
                product.Quantity -= quantity;

                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                return true;
            }
            else
            {
                Console.WriteLine("Insufficient quantity in inventory.");
                return false;
            }
        }

        public async Task CompleteSaleAsync(User cashier)
        {
            if (_currentSale != null && _currentSale.Cashier == cashier)
            {
                await _context.Sales.AddAsync(_currentSale);
                await _context.SaveChangesAsync();
                _currentSale = null;
            }
        }

        public Sale GetCurrentSale()
        {
            return _currentSale;
        }
    }
}
