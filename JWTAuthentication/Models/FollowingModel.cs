using System;
using System.Collections.Generic;

namespace JWTAuthentication.Authentication
{
    public class FollowingModel
    {
        public string ID { get; set; }
        public string UserID { get; set; }
        public string StoreID { get; set; }
        public DateTime FollowTime { get; set; }

    }
}
