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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public RatingController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        [Authorize]
        [HttpPost("AddRatingByUser")]
        public async Task<IActionResult> AddRatingByUserAsync(RatingModel rating)
        {
            try
            {
                var UserName = User.Identity.Name;
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string checkBuyQuery = $"SELECT * FROM Bill b inner join BillProduct bp on b.ID = bp.BillID WHERE b.BuyerID ='{user.Id}' AND bp.ProductID = '{rating.ProductID}' AND b.Status =1";
                    BillModel Bill = conn.Query<BillModel>(checkBuyQuery).FirstOrDefault();

                    if (Bill == null)
                    {
                        return StatusCode(StatusCodes.Status424FailedDependency, new { code = 424, message = "Không tìm thấy sản phẩm hoặc đơn chưa được giao" });
                    }

                    string query = $"INSERT INTO Rating (ID, Comment, Star, [Image], [Time], ProductID, UserID, [Like]) VALUES('{Guid.NewGuid()}', '{rating.Comment}', {rating.Star}, '{rating.Image}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', '{rating.ProductID}', '{user.Id}', 0)";
                    conn.Execute(query);

                    string getProductQuery = $"SELECT * FROM Product where ID = '{rating.ProductID}'";
                    ProductModel oldProduct = conn.Query<ProductModel>(getProductQuery).FirstOrDefault();

                    double newStar = (oldProduct.Star * oldProduct.RatingsCount + rating.Star) / (oldProduct.RatingsCount + 1);//trigger chinh sua new star
                    int newRatingCount = oldProduct.RatingsCount + 1;
                    string updateProductQuery = $"UPDATE Product SET  Star={newStar.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}, RatingsCount={newRatingCount} where id='{rating.ProductID}'";
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
                if (ratings.Count == 0) return StatusCode(StatusCodes.Status400BadRequest, new { code = 400, message = "Sản phẩm này chưa có rating" });
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra: " + ex.Message });
            }
        }

        public static List<RatingModel> _GetRatingForProduct(string ProductID)
        {
            using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
            {
                string query = $"SELECT * FROM Product p inner join Rating r on p.ID = r.ProductID WHERE p.ID = '{ProductID}'";
                List<RatingModel> ratings = conn.Query<RatingModel>(query).OrderByDescending(p => p.Time).AsList();
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

