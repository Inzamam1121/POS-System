using System;
using System.Collections.Generic;

namespace POS_System.Entities
{
    public class Purchase
    {
        public int PurchaseId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public List<ProductItem> Products { get; set; } = new List<ProductItem>();

        public Purchase()
        {
            PurchaseDate = DateTime.Now;
        }
         
        public Purchase(DateTime purchaseDate)
        {
            PurchaseDate = purchaseDate;
        }

        
    }
}
