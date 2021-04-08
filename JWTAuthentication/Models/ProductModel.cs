using System;
using System.Collections.Generic;

namespace JWTAuthentication.Authentication
{
    public class ProductModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Detail { get; set; }
        public string Description { get; set; }
        public string CategoryID { get; set; }
        public CategoryModel Category { get; set; }
        public int Discount { get; set; }
        public int Quanlity { get; set; }
        public string Image { get; set; }
        public DateTime AddedTime { get; set; }
        public DateTime LastModify { get; set; }
        public string StoreID { get; set; }
        public StoreModel Store { get; set; }
        public string SoldQuanlity { get; set; }

        public ICollection<CartModel> CartList { get; set; }
    }
}
