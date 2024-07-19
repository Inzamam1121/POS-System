using POS_System.Data;
using POS_System.Entities;
using System.Linq;

namespace POS_System.Services
{
    public class SaleTransactionService
    {
        private Sale _currentSale;

        public void StartNewSale(User cashier)
        {
            _currentSale = new Sale(cashier);
        }

        public bool AddProductToSale(Product product, int quantity, User cashier)
        {
            if (_currentSale == null || _currentSale.Cashier != cashier)
            {
                StartNewSale(cashier);
            }

            if (product.Quantity >= quantity)
            {
                _currentSale.AddProductToSale(product, quantity);
                product.Quantity -= quantity;
                return true;
            }
            else
            {
                Console.WriteLine("Insufficient quantity in inventory.");
                return false;
            }
        }

        public void CompleteSale(User cashier)
        {
            if (_currentSale != null && _currentSale.Cashier == cashier)
            {
                DataContext.Sales.Add(_currentSale);
                _currentSale = null;
            }
        }

        public Sale GetCurrentSale()
        {
            return _currentSale;
        }
    }
}
