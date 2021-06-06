using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Linq;

namespace JWTAuthentication.Authentication
{
    public class UsefulRatingModel
    {
        public string ID { get; set; }
        public string UserID { get; set; }
        public string RatingID { get; set; }

    }
}
