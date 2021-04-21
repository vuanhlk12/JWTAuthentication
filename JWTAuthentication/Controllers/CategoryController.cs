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
using Microsoft.AspNetCore.Http;

namespace JWTAuthentication.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        [HttpGet("CategoryAllChildList")]
        public IActionResult GetCategory(string ParentID = null)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra " + ex.Message });
            }
        }

        public List<CategoryModel> GetCategoryAllChildList(string ParentID = null)
        {
            try
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
            catch
            {
                return null;
            }
        }

        [HttpGet("CategoryChildList")]
        public IActionResult GetCategoryChildList(string ParentID = null)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string parentQuery = $"SELECT * FROM Category where id='{ParentID}'";

                    CategoryModel Parent = new CategoryModel();
                    if (ParentID != null)
                        Parent = conn.Query<CategoryModel>(parentQuery).FirstOrDefault();

                    string query = $"SELECT * FROM Category where parentid='{ParentID}'";
                    if (ParentID == null)
                    {
                        query = "SELECT * FROM Category where parentid is null";
                    }
                    var CategoryList = conn.Query<CategoryModel>(query).AsList();
                    Parent.ChildList = CategoryList;
                    return Ok(new { code = 200, message = Parent });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra " + ex.Message });
            }
        }

        [HttpGet("GetParentCategory")]
        public IActionResult GetParentCategory(string ParentID = null)
        {
            try
            {
                List<CategoryModel> category = _GetParentCategory(ParentID);
                return Ok(new { code = 200, message = category});
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra " + ex.Message });
            }
        }

        public List<CategoryModel> _GetParentCategory(string ParentID)
        {
            using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
            {
                string Query = $"SELECT * FROM Category where id='{ParentID}'";
                var Category = conn.Query<CategoryModel>(Query).FirstOrDefault();
                List<CategoryModel> ParentCategory = new List<CategoryModel>();
                if (Category.ParentID != null)
                {
                    ParentCategory = _GetParentCategory(Category.ParentID);
                }
                ParentCategory.Add(Category);
                return ParentCategory;
            }
        }
    }

}
