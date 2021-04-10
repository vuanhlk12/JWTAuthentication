using System;
using System.Collections.Generic;

namespace JWTAuthentication.Authentication
{
    public class CartModel
    {
        public string ID { get; set; }
        public string BuyerID { get; set; }
        public string ProductID { get; set; }
        public DateTime AddedTime { get; set; }
        public string Status { get; set; }
        public int Quantity { get; set; }

        public ProductModel Product { get; set; }

    }
}
