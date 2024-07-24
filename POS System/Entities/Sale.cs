using System;
using System.Collections.Generic;
using System.Linq;

namespace POS_System.Entities
{
    public enum SaleStatus
    {
        Start,
        Pending,
        Complete
    }

    public class Sale
    {
        public int SaleId { get; set; }
        public int CashierId { get; set; }
        public User Cashier { get; set; }
        public DateTime Date { get; set; }
        public SaleStatus Status { get; set; }
        public List<ProductItem> Products { get; set; } = new List<ProductItem>();
         
        public Sale() { }
         
        public Sale(User cashier, SaleStatus status)
        {
            Cashier = cashier;
            Date = DateTime.Now;
            Status = status;
        }

        public void AddProduct(Product product, int quantity)
        {
            var productItem = Products.Find(p => p.ProductId == product.ProductId);
            if (productItem == null)
            {
                Products.Add(new ProductItem(product, quantity));
            }
            else
            {
                productItem.AddQuantity(quantity);
            }
        }

        public decimal TotalAmount
        {
            get
            {
                return Products.Sum(pi => pi.TotalAmount);
            }
        }
    }
}
