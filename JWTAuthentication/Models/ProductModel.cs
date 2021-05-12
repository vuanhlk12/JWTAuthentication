using System;
using System.Collections.Generic;
using Nancy.Json;

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
        public List<string> Images
        {
            get
            {
                return new JavaScriptSerializer().Deserialize<List<string>>(Image);
            }
        }
        public DateTime AddedTime { get; set; }
        public DateTime LastModify { get; set; }
        public string StoreID { get; set; }
        public StoreModel Store { get; set; }
        public int SoldQuanlity { get; set; }
        public int RatingsCount { get; set; }
        public List<RatingModel> Ratings { get; set; }
        public double Star { get; set; }
    }
}
