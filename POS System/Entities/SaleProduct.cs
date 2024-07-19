using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_System.Entities
{
    public class SaleProduct
    {
        public List<Product> Products { get; set; }
        public List<int> Quantities { get; set; }

        public SaleProduct()
        {
            Products = new List<Product>();
            Quantities = new List<int>();
        }

        public void AddProduct(Product product, int quantity)
        {
            int index = Products.IndexOf(product);
            if (index >= 0)
            {
                Quantities[index] += quantity;
            }
            else
            {
                Products.Add(product);
                Quantities.Add(quantity);
            }
        }

        public decimal TotalAmount
        {
            get
            {
                decimal total = 0;
                for (int i = 0; i < Products.Count; i++)
                {
                    total += Products[i].Price * Quantities[i];
                }
                return total;
            }
        }
    }
}
