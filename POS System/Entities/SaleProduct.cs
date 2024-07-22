using System;
using System.Collections.Generic;
using System.Linq;

namespace POS_System.Entities
{
    public class SaleProduct
    {
        public int SaleProductId { get; set; }
        public int SaleId { get; set; }
        public Sale Sale { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }

        // Parameterless constructor for EF
        public SaleProduct() { }

        public void AddProduct(int quantity)
        {
            Quantity += quantity;
        }

        public decimal TotalAmount
        {
            get
            {
                return Product.Price * Quantity;
            }
        }
    }
}
