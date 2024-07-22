using System;
using System.Collections.Generic;
using System.Linq;

namespace POS_System.Entities
{
    public class Sale
    {
        public int SaleId { get; set; }
        public int CashierId { get; set; }
        public User Cashier { get; set; }
        public DateTime Date { get; set; }
        public List<SaleProduct> SaleProducts { get; set; } = new List<SaleProduct>();

        public Sale(User cashier)
        {
            Cashier = cashier;
            Date = DateTime.Now;
        }

        public Sale() { }

        public void AddProductToSale(Product product, int quantity)
        {
            var saleProduct = SaleProducts.FirstOrDefault(sp => sp.ProductId == product.Id);
            if (saleProduct == null)
            {
                saleProduct = new SaleProduct
                {
                    ProductId = product.Id,
                    Product = product,
                    Quantity = 0
                };
                SaleProducts.Add(saleProduct);
            }

            saleProduct.Quantity += quantity;
        }

        public decimal TotalAmount
        {
            get
            {
                return SaleProducts.Sum(sp => sp.Product.Price * sp.Quantity);
            }
        }
    }
}
