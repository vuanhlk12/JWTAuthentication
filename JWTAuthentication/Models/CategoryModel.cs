using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JWTAuthentication.Authentication
{
    public class CategoryModel
    {
        public string Id { get; set; }
        public string ParentID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public List<string> Images
        {
            get
            {
                return new JavaScriptSerializer().Deserialize<List<string>>(Image);
            }
        }
        public List<CategoryModel> ChildList { get; set; }
        public List<ProductModel> ProductsList { get; set; }

        public CategoryModel()
        {
            ChildList = new List<CategoryModel>();
            ProductsList = new List<ProductModel>();
        }


    }
}
