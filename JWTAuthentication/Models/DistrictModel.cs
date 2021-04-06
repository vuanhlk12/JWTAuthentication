using System;
using System.Collections.Generic;

namespace JWTAuthentication.Authentication
{
    public class DistrictModel
    {
        public string ID { get; set; }
        public string DistrictName { get; set; }
        public string CityID { get; set; }
        public DateTime LastModify { get; set; }
    }
}
