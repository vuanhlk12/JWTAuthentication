using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JWTAuthentication.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Http;

namespace JWTAuthentication.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        [HttpGet("GetProductByCategoryID")]
        public IActionResult GetProductByCategoryID(string CategoryID = null)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    CategoryController categoryController = new CategoryController();
                    List<CategoryModel> categories = categoryController.GetCategoryAllChildList(CategoryID);
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

                    CategoryModel category = new CategoryModel();

                    if (CategoryID != null)
                    {
                        string categoryQuery = $"SELECT  * FROM Category where ID ='{CategoryID}'";
                        category = conn.Query<CategoryModel>(categoryQuery).FirstOrDefault();
                    }

                    string query = $"SELECT * FROM Product WHERE CategoryID IN {keyStr}";
                    List<ProductModel> products = conn.Query<ProductModel>(query).AsList();

                    string countQuery = $"SELECT COUNT(*) FROM Product WHERE CategoryID IN {keyStr}";
                    int count = conn.Query<int>(countQuery).FirstOrDefault();

                    category.ProductsList = products;
                    return Ok(new { code = 200, total = count, message = category });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra " + ex.Message });
            }
        }

        public List<ProductModel> GetProductByCategoryID1(string CategoryID = null)
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
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("GetProductByCategoryIDbyRange")]
        public IActionResult GetProductByCategoryIDbyRange(int size, int page, string CategoryID = null)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    CategoryController categoryController = new CategoryController();
                    List<CategoryModel> categories = categoryController.GetCategoryAllChildList(CategoryID);
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

                    CategoryModel category = new CategoryModel();

                    if (CategoryID != null)
                    {
                        string categoryQuery = $"SELECT  * FROM Category where ID ='{CategoryID}'";
                        category = conn.Query<CategoryModel>(categoryQuery).FirstOrDefault();
                    }

                    string query = $"FROM Product WHERE CategoryID IN {keyStr}";
                    query = $"SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY id) RowNr, * {query} ) t WHERE RowNr BETWEEN {size * page} AND {size * (page + 1)}";
                    List<ProductModel> products = conn.Query<ProductModel>(query).AsList();

                    string countQuery = $"SELECT COUNT(*) FROM Product WHERE CategoryID IN {keyStr}";
                    int count = conn.Query<int>(countQuery).FirstOrDefault();

                    category.ProductsList = products ?? new List<ProductModel>();
                    return Ok(new { code = 200, total = count, message = category });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra " + ex.Message });
            }
        }

        [HttpGet("GetProductByID")]
        public IActionResult GetProductByID(string ProductID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string query = $"SELECT * FROM Product WHERE ID = '{ProductID}'";
                    ProductModel products = conn.Query<ProductModel>(query).FirstOrDefault();
                    return Ok(new { code = 200, message = products });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra " + ex.Message });
            }
        }

        [HttpPost("AddProductByStore")]
        public ActionResult AddProductByStore([FromBody] ProductModel Product)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string query = $"INSERT INTO Product (ID,Name,Price,Color,[Size],Detail,Description,CategoryID,Discount,Quanlity,[Image],AddedTime,LastModify,StoreID,SoldQuanlity) VALUES (N'{Guid.NewGuid()}', N'{Product.Name}', {Product.Price}, N'{Product.Color}', N'{Product.Size}', N'{Product.Detail}', N'{Product.Description}', N'{Product.CategoryID}', {Product.Discount}, {Product.Quanlity}, N'{Product.Image}', '{DateTime.Now.ToString("yyyy-MM-dd")}', '{DateTime.Now.ToString("yyyy-MM-dd")}', N'{Product.StoreID}', 0); ";
                    conn.Execute(query);
                    return Ok(new { code = 200, message = $"Thêm sản phẩm {Product.Name} thành công" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra: " + ex.Message });
            }
        }
    }

}
