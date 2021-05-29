using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JWTAuthentication.Authentication
{
    public class StoreModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public DateTime CreateTime { get; set; }
        public string OwnerID { get; set; }
        public UserModel Owner
        {
            get
            {
                return getStoreOwnerUsername();
            }
        }
        public double Star { get; set; }
        public int RatingsCount { get; set; }
        public List<RatingModel> Ratings { get; set; }
        public int FollowerCount { get; set; }
        public List<UserModel> Followers { get; set; }
        public int Approved { get; set; }
        public UserModel getStoreOwnerUsername()
        {
            using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
            {
                try
                {
                    string query = $"SELECT * FROM AspNetUsers WHERE Id = '{OwnerID}'";
                    UserModel user = conn.Query<UserModel>(query).AsList().FirstOrDefault();
                    return user;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}
