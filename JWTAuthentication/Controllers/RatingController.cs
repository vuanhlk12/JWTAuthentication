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
                    string checkCartQuery = $"SELECT COUNT(*) FROM Cart WHERE id = '{rating.CartID}' AND Status = 'Paid'";
                    int isValid = conn.Query<int>(checkCartQuery).FirstOrDefault();

                    if (isValid == 0)
                    {
                        return StatusCode(StatusCodes.Status424FailedDependency, new { code = 424, message = "Không tìm thấy mặt hàng trong giỏ hoặc đơn chưa được thanh toán" });
                    }

                    string query = $"INSERT INTO Rating (ID, Comment, Star, ParentID, [Image], [Time], CartID) VALUES('{Guid.NewGuid()}', '{rating.Comment}', {rating.Star}, '{rating.ParentID}', '{rating.Image}', '{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}', '{rating.CartID}')";
                    conn.Execute(query);

                    return Ok(new
                    {
                        code = 200,
                        message = "Đã thêm rating thành công"
                    });
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra" });
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

    }
}

