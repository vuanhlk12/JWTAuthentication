using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JWTAuthentication.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.Data.SqlClient;
using Dapper;

namespace JWTAuthentication.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        [HttpGet("GetProductByCategoryID")]
        public List<ProductModel> GetProductByCategoryID(string CategoryID = null)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    CategoryController category = new CategoryController();
                    List<CategoryModel> categories = category.GetCategoryAllChildList(CategoryID);
                    string keyStr = "";

                    if (categories.Count == 0)
                    {
                        keyStr = $"('{CategoryID}')";
                    }
                    else
                    {
                        keyStr = string.Join("','", categories.Select(item => item.Id.ToString()));
                        keyStr = "('" + keyStr + "')";
                    }

                    string query = $"SELECT * FROM Product WHERE CategoryID IN {keyStr}";
                    List<ProductModel> products = conn.Query<ProductModel>(query).AsList();
                    return products;
                }
            }
            catch
            {
                return null;
            }
        }
    }

}
