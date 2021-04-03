﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JWTAuthentication.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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
            using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
            {
                CategoryController category = new CategoryController();
                List<CategoryModel> categories = category.GetCategoryAllChildList(CategoryID);
                string keyStr = "";
                keyStr = string.Join("','", categories.Select(item => item.Id.ToString()));
                keyStr = "('" + keyStr + "')";

                if (categories.Count == 0)
                {
                    keyStr = $"('{CategoryID}')";
                }

                string query = $"SELECT * FROM Product WHERE CategoryID IN {keyStr}";
                List<ProductModel> products = conn.Query<ProductModel>(query).AsList();
                return products;
            }
        }
    }

}
