using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTAuthentication.Authentication
{
    public class HistoryBillStoreModel
    {
        public int ProductsTotal { get; set; }
        public string BillID { get; set; }
        public string BuyerID { get; set; }
        public string BuyerAccount { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime? ShipTime { get; set; }
        public string AddressID { get; set; }
        public int Status { get; set; }
        //public int ProductsTotal
        //{
        //    get
        //    {
        //        return Products.Sum(item => (int)(item.Quanlity * item.Price * (1 - (float)item.Discount / 100)));
        //    }
        //}

        public string PaymentID { get; set; }
        public PaymentModel Payment
        {
            get
            {
                return getPayment();
            }
        }

        public PaymentModel getPayment()
        {
            using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
            {
                try
                {
                    string query = $"SELECT * FROM Payment WHERE id = '{PaymentID}'";
                    PaymentModel user = conn.Query<PaymentModel>(query).AsList().FirstOrDefault();
                    return user;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        public List<ProductModel> Products { get; set; }

    }
}
