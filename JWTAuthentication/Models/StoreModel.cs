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
        public ApplicationUser Owner { get; set; }
    }
}
