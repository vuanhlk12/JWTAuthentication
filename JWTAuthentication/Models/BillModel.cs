using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Linq;

namespace JWTAuthentication.Authentication
{
    public class BillModel
    {
        public string ID { get; set; }
        public string BuyerID { get; set; }
        public string ListItem { get; set; }
        public int Total { get; set; }
        public string OrderTime { get; set; }
        public DateTime ShipTime { get; set; }
        public int Status { get; set; }
        public string PaymentID { get; set; }

    }
}
