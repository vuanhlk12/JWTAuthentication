using System;
using System.Collections.Generic;

namespace JWTAuthentication.Authentication
{
    public class AddressModel
    {
        public string ID { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string UserID { get; set; }
        public string DistrictID { get; set; }
        public int IsDefault { get; set; }
        public CityModel City { get; set; }
        public DistrictModel District { get; set; }
    }
}
