using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTAuthentication.Authentication
{
    public class BillProductModel
    {
        public string ID { get; set; }
        public string BillID { get; set; }
        public string ProductID {get;set;}
        public int ProductQuantity { get; set; }
        public DateTime TransactionTime { get; set; }
        public string StoreID { get; set; }

        public ProductModel Product { get; set; }
    }
}
