using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
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
using Newtonsoft.Json;

namespace JWTAuthentication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RatingController : ControllerBase
    {
        [HttpGet("AddRatingByUser")]
        public IActionResult AddRatingByUser(RatingModel rating)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string checkCartQuery = $"SELECT * FROM Cart WHERE id = '{rating.CartID}' AND Status = 'Paid'";
                    CartModel cart = conn.Query<CartModel>(checkCartQuery).FirstOrDefault();

                    if (cart == null)
                    {
                        return StatusCode(StatusCodes.Status424FailedDependency, new { code = 424, message = "Không tìm thấy mặt hàng trong giỏ hoặc đơn chưa được thanh toán" });
                    }

                    string query = $"INSERT INTO Rating (ID, Comment, Star, ParentID, [Image], [Time], CartID) VALUES('{Guid.NewGuid()}', '{rating.Comment}', {rating.Star}, '{rating.ParentID}', '{rating.Image}', '{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}', '{rating.CartID}')";
                    conn.Execute(query);

                    string getProductQuery = $"SELECT * FROM Product where ID = '{cart.ProductID}'";
                    ProductModel oldProduct = conn.Query<ProductModel>(getProductQuery).FirstOrDefault();

                    double newStar = (oldProduct.Star * oldProduct.RatingsCount + rating.Star) / (oldProduct.RatingsCount + 1);//trigger chinh sua new star
                    int newRatingCount = oldProduct.RatingsCount + 1;
                    string updateProductQuery = $"UPDATE Product SET  Star={newStar.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}, RatingsCount={newRatingCount} where id='{cart.ProductID}'";
                    conn.Execute(updateProductQuery);

                    string getStoreQuery = $"SELECT * from Store where ID = '{oldProduct.StoreID}'";
                    StoreModel oldStore = conn.Query<StoreModel>(getStoreQuery).FirstOrDefault();

                    newStar = (oldStore.Star * oldStore.RatingsCount + rating.Star) / (oldStore.RatingsCount + 1);//trigger chinh sua new star
                    newRatingCount = oldStore.RatingsCount + 1;
                    string updateStoreQuery = $"UPDATE Store SET  Star={newStar.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}, RatingsCount={newRatingCount} where id='{oldStore.ID}'";
                    conn.Execute(updateStoreQuery);

                    return Ok(new
                    {
                        code = 200,
                        message = "Đã thêm rating thành công"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra: " + ex.Message });
            }
        }

        [HttpGet("GetRatingForProduct")]
        public IActionResult GetRatingForProduct(string ProductID)
        {
            try
            {
                var ratings = _GetRatingForProduct(ProductID);
                float starSum = 0;
                foreach (var rating in ratings)
                {
                    starSum += rating.Star;
                }

                return Ok(new
                {
                    code = 200,
                    avgStar = starSum / ratings.Count,
                    message = ratings
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra" });
            }
        }

        public List<RatingModel> _GetRatingForProduct(string ProductID)
        {
            using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
            {
                string query = $"SELECT r.* FROM Product p inner join Cart c on p.ID = c.ProductID inner join Rating r on c.ID = r.CartID WHERE p.ID = '{ProductID}'";
                List<RatingModel> ratings = conn.Query<RatingModel>(query).AsList();
                return ratings;
            }
        }

        public (double, int) _GetSmallRatingForProduct(string ProductID)
        {
            using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
            {
                string query = $"SELECT r.* FROM Product p inner join Cart c on p.ID = c.ProductID inner join Rating r on c.ID = r.CartID WHERE p.ID = '{ProductID}'";
                List<RatingModel> ratings = conn.Query<RatingModel>(query).AsList();

                if (ratings.Count == 0) return (0, 0);

                float starSum = 0;
                foreach (var rating in ratings)
                {
                    starSum += rating.Star;
                }
                return (starSum / ratings.Count, ratings.Count);
            }
        }

    }
}

