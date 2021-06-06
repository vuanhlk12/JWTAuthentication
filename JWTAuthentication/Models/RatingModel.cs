using Dapper;
using JWTAuthentication.Controllers;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JWTAuthentication.Authentication
{
    public class RatingModel
    {
        public string ID { get; set; }
        public string Comment { get; set; }
        public int Star { get; set; }
        public string Image { get; set; }
        public DateTime Time { get; set; }
        public string ProductID { get; set; }
        public string UserID { get; set; }
        public int Like { get; set; }
        public bool Liked { get; set; }
        public UserModel User
        {
            get
            {
                return UserController._GetUserByUserID(UserID);
            }
        }
        public bool checkLiked(string UserID)
        {
            using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
            {
                try
                {
                    string checkUseful = $"SELECT * FROM UsefulRating WHERE UserID ='{UserID}' AND RatingID='{ID}'";
                    var togleLike = conn.Query<RatingModel>(checkUseful).FirstOrDefault();
                    return (togleLike != null);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

    }
}
