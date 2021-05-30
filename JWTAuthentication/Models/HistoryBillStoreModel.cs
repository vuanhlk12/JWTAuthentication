using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTAuthentication.Authentication
{
    public class HistoryBillStoreModel
    {
        public string BillID { get; set; }
        public string BuyerID { get; set; }
        public string BuyerAccount { get; set; }
        public DateTime OrderTime { get; set; }
        public string AddressID { get; set; }
        public int Status { get; set; }
        public int ProductsTotal
        {
            get
            {
                return Products.Sum(item => (int)(item.Quanlity * item.Price * (1 - (float)item.Discount / 100)));
            }
        }
        public List<ProductModel> Products { get; set; }

    }
}
