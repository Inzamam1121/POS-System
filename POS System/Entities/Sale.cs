using System;
using System.Collections.Generic;
using System.Linq;

namespace POS_System.Entities
{
    public class Sale
    {
        public User Cashier { get; set; }
        public DateTime Date { get; set; }
        public List<SaleProduct> SaleProducts { get; set; }

        public Sale(User cashier)
        {
            Cashier = cashier;
            Date = DateTime.Now;
            SaleProducts = new List<SaleProduct>();
        }

        public void AddProductToSale(Product product, int quantity)
        {
            var saleProduct = SaleProducts.FirstOrDefault(sp => sp.Products.Contains(product));
            if (saleProduct == null)
            {
                saleProduct = new SaleProduct();
                SaleProducts.Add(saleProduct);
            }

            saleProduct.AddProduct(product, quantity);
        }

        public decimal TotalAmount
        {
            get
            {
                return SaleProducts.Sum(sp => sp.TotalAmount);
            }
        }
    }
}

