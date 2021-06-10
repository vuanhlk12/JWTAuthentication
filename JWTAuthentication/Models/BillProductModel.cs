using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTAuthentication.Authentication
{
    public class BillProductModel
    {
        public string ShippedProductID { get; set; }
        public int ProductsTotal { get; set; }
        public string ID { get; set; }
        public string BillID { get; set; }
        public string BuyerID { get; set; }
        public string BuyerAccount { get; set; }
        public DateTime OrderTime { get; set; }
        public string ProductID { get; set; }
        public int ProductQuantity { get; set; }
        public DateTime TransactionTime { get; set; }
        public DateTime? ShipTime { get; set; }
        public string StoreID { get; set; }
        public string AddressID { get; set; }
        public int BillStatus { get; set; }
        public ProductModel Product { get; set; }
        //public int ProductsTotal
        //{
        //    get
        //    {
        //        return Products.Sum(item => (int)(item.Quanlity * item.Price * (1 - (float)item.Discount / 100)));
        //    }
        //}
        public string PaymentID { get; set; }
        
        public List<ProductModel> Products { get; set; }


    }
}
