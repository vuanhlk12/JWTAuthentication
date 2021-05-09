using System;
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
    }
}
