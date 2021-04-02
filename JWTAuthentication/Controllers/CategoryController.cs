using System;
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
    public class CategoryController : ControllerBase
    {
        [HttpGet("CategoryAllChildList")]
        public IActionResult GetCategory(string ParentID = null)
        {
            using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
            {
                string query = $"SELECT * FROM Category where id='{ParentID}'";

                CategoryModel category = conn.Query<CategoryModel>(query).FirstOrDefault();
                if (ParentID == null)
                {
                    category = new CategoryModel();
                }
                List<CategoryModel> a = GetCategoryAllChildList(ParentID);
                category.ChildList = a.BuildTree().ChildList;
                return Ok(new
                {
                    code = 200,
                    Category = category
                });
            }
        }

        public List<CategoryModel> GetCategoryAllChildList(string ParentID = null)
        {
            using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
            {
                List<CategoryModel> ReturnList = new List<CategoryModel>();
                string query = $"SELECT * FROM Category where parentid='{ParentID}'";
                if (ParentID == null)
                {
                    query = "SELECT * FROM Category where parentid is null";
                }
                var CategoryList = conn.Query<CategoryModel>(query).AsList();
                ReturnList.AddRange(CategoryList);
                if (CategoryList.Count > 0)
                {
                    foreach (CategoryModel category in CategoryList)
                    {
                        ReturnList.AddRange(GetCategoryAllChildList(category.Id));
                    }
                }
                return ReturnList;
            }
        }

        [HttpGet("CategoryChildList")]
        public List<CategoryModel> GetCategoryChildList(string ParentID = null)
        {
            using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
            {
                List<CategoryModel> ReturnList = new List<CategoryModel>();
                string query = $"SELECT * FROM Category where parentid='{ParentID}'";
                if (ParentID == null)
                {
                    query = "SELECT * FROM Category where parentid is null";
                }
                var CategoryList = conn.Query<CategoryModel>(query).AsList();
                ReturnList.AddRange(CategoryList);
                return ReturnList;
            }
        }
    }

}
