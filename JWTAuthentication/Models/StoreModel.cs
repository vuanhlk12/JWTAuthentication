using System;
using System.Collections.Generic;

namespace JWTAuthentication.Authentication
{
    public class StoreModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public DateTime CreateTime { get; set; }
        public string OwnerID { get; set; }
        public UserModel Owner { get; set; }
        public double Star { get; set; }
        public List<RatingModel> Ratings { get; set; }
        public List<UserModel> UsersFollow { get; set; }
    }
}
