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
using Nancy.Json;

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
        public IActionResult GetProductByCategoryIDbyRange(int size, int page, string CategoryID = null, int star = 0, int fromPrice = 0, int toPrice = int.MaxValue)
        {
            page--;
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    CategoryModel category = new CategoryModel();
                    string query;
                    List<ProductModel> products;
                    string countQuery;
                    int count;

                    if (CategoryID != null)
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

                        string categoryQuery = $"SELECT  * FROM Category where ID ='{CategoryID}'";
                        category = conn.Query<CategoryModel>(categoryQuery).FirstOrDefault();
                        query = $"FROM Product WHERE CategoryID IN {keyStr}";

                    }
                    else
                    {
                        query = $"FROM Product WHERE 1=1";


                    }
                    if (star != 0) query += $"and Star >={star} and Star <{star + 1}";
                    if (fromPrice >= 0) query += $"and Price >={fromPrice}";
                    if (toPrice <= int.MaxValue) query += $"and Price <={toPrice}";

                    countQuery = $"SELECT COUNT(*) {query}";
                    count = conn.Query<int>(countQuery).FirstOrDefault();

                    query = $"SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY id) RowNr, * {query} ) t WHERE RowNr BETWEEN {size * page + 1} AND {size * (page + 1)}";
                    products = conn.Query<ProductModel>(query).AsList();
                    //foreach (var product in products)
                    //{
                    //    double avgStar = 0;
                    //    int ratingCount = 0;
                    //    (avgStar, ratingCount) = new RatingController()._GetSmallRatingForProduct(product.ID);
                    //    product.Star = avgStar;
                    //    product.RatingsCount = ratingCount;
                    //}

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

                    string storeQuery = $"SELECT * FROM Store WHERE id = '{products.StoreID}'";
                    StoreModel store = conn.Query<StoreModel>(storeQuery).FirstOrDefault();

                    string ratingQuery = $"SELECT r.* FROM Product p inner join Cart c on p.ID = c.ProductID inner join Rating r on c.ID =r.CartID WHERE p.ID = '{ProductID}'";
                    List<RatingModel> ratings = conn.Query<RatingModel>(ratingQuery).AsList();

                    float starSum = 0;
                    foreach (var rating in ratings)
                    {
                        starSum += rating.Star;
                    }

                    products.Store = store;
                    products.Ratings = ratings;
                    if (ratings.Count == 0)
                        products.Star = 0;
                    else
                        products.Star = starSum / ratings.Count;
                    return Ok(new
                    {
                        code = 200,
                        message = products
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra " + ex.Message });
            }
        }

        [HttpGet("GetProductByStoreIDbyRange")]
        public IActionResult GetProductByStoreIDbyRange(int size, int page, string StoreID = null, int star = 0, int fromPrice = 0, int toPrice = int.MaxValue)
        {
            page--;
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string query = $"FROM Product WHERE StoreID ='{StoreID}'";
                    if (star != 0) query += $"and Star >={star} and Star <{star + 1}";
                    if (fromPrice >= 0) query += $"and Price >={fromPrice}";
                    if (toPrice <= int.MaxValue) query += $"and Price <={toPrice}";

                    string countQuery = $"SELECT COUNT(*) {query}";
                    int count = conn.Query<int>(countQuery).FirstOrDefault();

                    query = $"SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY id) RowNr, * {query} ) t WHERE RowNr BETWEEN {size * page + 1} AND {size * (page + 1)}";
                    List<ProductModel> products = conn.Query<ProductModel>(query).AsList();

                    return Ok(new { code = 200, total = count, message = products });
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
                    string query = $"INSERT INTO Product (ID,Name,Price,Color,[Size],Detail,Description,CategoryID,Discount,Quanlity,[Image],AddedTime,LastModify,StoreID,SoldQuanlity,Star,RatingsCount) VALUES (N'{Guid.NewGuid()}', N'{Product.Name}', {Product.Price}, N'{Product.Color}', N'{Product.Size}', N'{Product.Detail}', N'{Product.Description}', N'{Product.CategoryID}', {Product.Discount}, {Product.Quanlity}, N'{Product.Image}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', N'{Product.StoreID}', 0,0,0); ";
                    conn.Execute(query);
                    return Ok(new { code = 200, message = $"Thêm sản phẩm {Product.Name} thành công" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra: " + ex.Message });
            }
        }

        [HttpPost("UpdateProductByStore")]
        public ActionResult UpdateProductByStore([FromBody] ProductModel Product)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string query = $"UPDATE Product SET Name=N'{Product.Name}', Price={Product.Price}, Color=N'{Product.Color}', [Size]=N'{Product.Size}', Detail=N'{Product.Detail}', Description=N'{Product.Description}', CategoryID=N'{Product.CategoryID}', Discount={Product.Discount}, Quanlity={Product.Quanlity}, [Image]=N'{Product.Image}', LastModify='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', StoreID=N'{Product.StoreID}' WHERE ID='{Product.ID}'";
                    conn.Execute(query);
                    return Ok(new { code = 200, message = $"Sửa sản phẩm {Product.Name} thành công" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra: " + ex.Message });
            }
        }

        [HttpPost("DeleteProductByStore")]
        public ActionResult DeleteProductByStore([FromBody] string ProductID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string query = $"DELETE FROM Product WHERE ID='{ProductID}'";
                    conn.Execute(query);
                    return Ok(new { code = 200, message = $"Xóa thành công" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra: " + ex.Message });
            }
        }
    }

}
