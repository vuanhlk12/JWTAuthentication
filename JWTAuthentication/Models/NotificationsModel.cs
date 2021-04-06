using System;
using System.Collections.Generic;

namespace JWTAuthentication.Authentication
{
    public class NotificationsModel
    {
        public string ID { get; set; }
        public string UserID { get; set; }
        public string Notification { get; set; }
        public DateTime Time { get; set; }
        public string Status { get; set; }
        public string CartID { get; set; }
    }
}
