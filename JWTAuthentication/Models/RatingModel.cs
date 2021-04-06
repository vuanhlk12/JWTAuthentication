using System;
using System.Collections.Generic;

namespace JWTAuthentication.Authentication
{
    public class RatingModel
    {
        public string ID { get; set; }
        public string Comment { get; set; }
        public int Star { get; set; }
        public string ParentID { get; set; }
        public string Image { get; set; }
        public DateTime Time { get; set; }
        public string CartID { get; set; }

    }
}
