using Dapper;
using JWTAuthentication.Controllers;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JWTAuthentication.Authentication
{
    public class ColumnGraph
    {
        public DateTime? date { get; set; }
        public double value { get; set; }

    }

    public class DonutGraph
    {
        public string CategoryID { get; set; }
        public CategoryModel Category
        {
            get
            {
                return getCategory();
            }
        }
        public DateTime? date { get; set; }
        public double value { get; set; }
        public CategoryModel getCategory()
        {
            using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
            {
                try
                {
                    string query = $"SELECT * from Category WHERE Id = '{CategoryID}'";
                    CategoryModel cate = conn.Query<CategoryModel>(query).AsList().FirstOrDefault();
                    return cate;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

    }
    public class DonutResult
    {
        public string Name { get; set; }
        public int Value { get; set; }

    }
}
