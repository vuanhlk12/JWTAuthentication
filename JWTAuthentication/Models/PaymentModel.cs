using System;
using System.Collections.Generic;

namespace JWTAuthentication.Authentication
{
    public class PaymentModel
    {
        public string ID { get; set; }
        public string Type { get; set; }
        public string Detail { get; set; }
        public string UserID { get; set; }
    }
}
