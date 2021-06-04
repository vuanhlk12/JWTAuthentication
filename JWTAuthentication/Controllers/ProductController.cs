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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace JWTAuthentication.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public ProductController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }


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
        public IActionResult GetProductByCategoryIDbyRange(int size, int page, string CategoryID = null, int star = 0, int fromPrice = 0, int toPrice = int.MaxValue, string searchKey = null, string StoreID = null)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    CategoryModel category = new CategoryModel();
                    string query;
                    List<ProductModel> products;

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


                    //query = $"SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY id) RowNr, * {query} ) t WHERE RowNr BETWEEN {size * page + 1} AND {size * (page + 1)}";
                    query = $"SELECT  * {query}";

                    var listAll = conn.Query<ProductModel>(query).AsList();
                    if (StoreID != null)
                    {
                        listAll = listAll.Where(p => p.StoreID == StoreID).ToList();
                    }

                    if (searchKey != null)
                    {
                        listAll = SearchByName(listAll, searchKey);
                    }

                    int count = listAll.Count();
                    products = listAll.OrderBy(p => p.ID).Skip(size * page).Take(size).AsList();

                    category.ProductsList = products ?? new List<ProductModel>();
                    return Ok(new { code = 200, total = count, message = category });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra " + ex.Message });
            }
        }

        public List<ProductModel> SearchByName(List<ProductModel> listAll, string searchKey)
        {
            string[] words = searchKey.Split(' ');
            List<ProductModel> returnList = new List<ProductModel>();
            foreach (ProductModel product in listAll)
            {
                int count = 0;
                foreach (string word in words)
                {
                    if (product.Name.ToLower().Contains(word.ToLower()))
                    {
                        count++;
                    }
                }
                if (count == words.Count())
                    returnList.Add(product);
            }
            return returnList;
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

                    List<RatingModel> ratings = RatingController._GetRatingForProduct(ProductID);

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
        public IActionResult GetProductByStoreIDbyRange(int size, int page, string StoreID = null, int star = 0, int fromPrice = 0, int toPrice = int.MaxValue, string CategoryID = null)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string query = $"FROM Product WHERE StoreID ='{StoreID}'";
                    if (star != 0) query += $"and Star >={star} and Star <{star + 1}";
                    if (fromPrice >= 0) query += $"and Price >={fromPrice}";
                    if (toPrice <= int.MaxValue) query += $"and Price <={toPrice}";

                    query = $"SELECT  * {query}";
                    var listAll = conn.Query<ProductModel>(query).AsList();

                    if (CategoryID != null)
                    {
                        CategoryController categoryController = new CategoryController();
                        List<CategoryModel> categories = categoryController.GetCategoryAllChildList(CategoryID);
                        foreach (CategoryModel category in categories)
                        {
                            listAll = listAll.Where(p => p.CategoryID == category.Id).ToList();
                        }
                    }

                    int count = listAll.Count();
                    List<ProductModel> products = listAll.OrderBy(p => p.ID).Skip(size * page).Take(size).AsList();

                    return Ok(new { code = 200, total = count, message = products });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra " + ex.Message });
            }
        }


        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Seller)]
        [HttpPost("AddProductByStore")]
        public async System.Threading.Tasks.Task<ActionResult> AddProductByStoreAsync([FromBody] ProductModel Product)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string checkValidStore = $"SELECT * FROM Store WHERE OwnerID= '{user.Id}'";
                    var store = conn.Query<StoreModel>(checkValidStore).FirstOrDefault();
                    if (store.Approved != 1) return StatusCode(StatusCodes.Status406NotAcceptable, new { code = 406, message = "Cửa hàng đang bị khóa hoặc chưa được chấp nhận" });

                    Product.Name = Product.Name.Replace("'", "''");
                    Product.Color = Product.Color.Replace("'", "''");
                    Product.Size = Product.Size.Replace("'", "''");
                    Product.Image = Product.Image.Replace("'", "''");
                    Product.Description = Product.Description.Replace("'", "''");
                    Product.Detail = Product.Detail.Replace("'", "''");

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

        [Authorize(Roles = UserRoles.Seller)]
        [HttpPost("UpdateProductByStore")]
        public async System.Threading.Tasks.Task<ActionResult> UpdateProductByStoreAsync([FromBody] ProductModel Product)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string checkValidStore = $"SELECT * FROM Store WHERE OwnerID= '{user.Id}'";
                    var store = conn.Query<StoreModel>(checkValidStore).FirstOrDefault();
                    if (store.Approved != 1) return StatusCode(StatusCodes.Status406NotAcceptable, new { code = 406, message = "Cửa hàng đang bị khóa hoặc chưa được chấp nhận" });

                    string productQuery = $"SELECT * FROM Product WHERE ID='{Product.ID}' AND StoreID ='{store.ID}'";
                    var oldProduct = conn.Query<ProductModel>(productQuery).FirstOrDefault();
                    if (oldProduct == null) return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Không tìm thấy sản phẩm hoặc cửa hàng không có quyền sửa sản phẩm này" });

                    Product.Name = Product.Name.Replace("'", "''");
                    Product.Color = Product.Color.Replace("'", "''");
                    Product.Size = Product.Size.Replace("'", "''");
                    Product.Image = Product.Image.Replace("'", "''");
                    Product.Description = Product.Description.Replace("'", "''");
                    Product.Detail = Product.Detail.Replace("'", "''");

                    string query = $"UPDATE Product SET Name=N'{(string.IsNullOrEmpty(Product.Name) ? oldProduct.Name : Product.Name)}', Price={(Product.Price ?? oldProduct.Price)}, Color=N'{(string.IsNullOrEmpty(Product.Color) ? oldProduct.Color : Product.Color)}', [Size]=N'{(string.IsNullOrEmpty(Product.Size) ? oldProduct.Size : Product.Size)}', Detail=N'{(string.IsNullOrEmpty(Product.Detail) ? oldProduct.Detail : Product.Detail)}', Description=N'{(string.IsNullOrEmpty(Product.Description) ? oldProduct.Description : Product.Description)}', CategoryID=N'{(string.IsNullOrEmpty(Product.CategoryID) ? oldProduct.CategoryID : Product.CategoryID)}', Discount={(Product.Discount ?? oldProduct.Discount)}, Quanlity={(Product.Quanlity ?? oldProduct.Quanlity)}, [Image]=N'{(string.IsNullOrEmpty(Product.Image) ? oldProduct.Image : Product.Image)}', LastModify='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE ID='{Product.ID}'";
                    conn.Execute(query);
                    return Ok(new { code = 200, message = $"Sửa sản phẩm {Product.Name} thành công" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra: " + ex.Message });
            }
        }

        [Authorize(Roles = UserRoles.Admin )]
        [HttpPost("UpdateProductByAdmin")]
        public async System.Threading.Tasks.Task<ActionResult> UpdateProductByAdmin([FromBody] ProductModel Product)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string productQuery = $"SELECT * FROM Product WHERE ID='{Product.ID}'";
                    var oldProduct = conn.Query<ProductModel>(productQuery).FirstOrDefault();
                    if (oldProduct == null) return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Không tìm thấy sản phẩm" });

                    Product.Name = Product.Name.Replace("'", "''");
                    Product.Color = Product.Color.Replace("'", "''");
                    Product.Size = Product.Size.Replace("'", "''");
                    Product.Image = Product.Image.Replace("'", "''");
                    Product.Description = Product.Description.Replace("'", "''");
                    Product.Detail = Product.Detail.Replace("'", "''");

                    string query = $"UPDATE Product SET Name=N'{(string.IsNullOrEmpty(Product.Name) ? oldProduct.Name : Product.Name)}', Price={(Product.Price ?? oldProduct.Price)}, Color=N'{(string.IsNullOrEmpty(Product.Color) ? oldProduct.Color : Product.Color)}', [Size]=N'{(string.IsNullOrEmpty(Product.Size) ? oldProduct.Size : Product.Size)}', Detail=N'{(string.IsNullOrEmpty(Product.Detail) ? oldProduct.Detail : Product.Detail)}', Description=N'{(string.IsNullOrEmpty(Product.Description) ? oldProduct.Description : Product.Description)}', CategoryID=N'{(string.IsNullOrEmpty(Product.CategoryID) ? oldProduct.CategoryID : Product.CategoryID)}', Discount={(Product.Discount ?? oldProduct.Discount)}, Quanlity={(Product.Quanlity ?? oldProduct.Quanlity)}, [Image]=N'{(string.IsNullOrEmpty(Product.Image) ? oldProduct.Image : Product.Image)}', LastModify='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE ID='{Product.ID}'";
                    conn.Execute(query);
                    return Ok(new { code = 200, message = $"Sửa sản phẩm {Product.Name} thành công" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra: " + ex.Message });
            }
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Seller)]
        [HttpPost("DeleteProductByStore")]
        public async System.Threading.Tasks.Task<ActionResult> DeleteProductByStoreAsync([FromBody] string ProductID)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {

                    string getProduct = $"SELECT * FROM Product WHERE ID= '{ProductID}'";
                    var product = conn.Query<ProductModel>(getProduct).FirstOrDefault();

                    string checkValidStore = $"SELECT * FROM Store WHERE OwnerID= '{user.Id}'";
                    var store = conn.Query<StoreModel>(checkValidStore).FirstOrDefault();
                    if (store.Approved != 1) return StatusCode(StatusCodes.Status406NotAcceptable, new { code = 406, message = "Cửa hàng đang bị khóa hoặc chưa được chấp nhận" });

                    string productQuery = $"SELECT * FROM Product WHERE ID='{ProductID}' AND StoreID ='{store.ID}'";
                    var oldProduct = conn.Query<ProductModel>(productQuery).FirstOrDefault();
                    if (oldProduct == null) return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Không tìm thấy sản phẩm hoặc cửa hàng không có quyền xóa sản phẩm này" });

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
