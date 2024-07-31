using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace POS_System.Entities
{
    public class ProductItem
    {
        public int ProductItemId { get; set; }
        public int SaleId { get; set; }
        public Sale Sale { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Purchase Purchase { get; set; }
        public int PurchaseId { get; set; }
        public ProductItem() { }
         
        public ProductItem(int productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public ProductItem(Product product, int quantity)
        {
            Product = product;
            ProductId = product.ProductId;
            Quantity = quantity;
        }

        public void AddQuantity(int quantity)
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
