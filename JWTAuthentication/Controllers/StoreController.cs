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
    public class StoreController : ControllerBase
    {
        [HttpGet("GetStoreByUserID")]
        public IActionResult GetStoreByUserID(string UserID = null)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string query = $"SELECT * FROM Store WHERE OwnerID = '{UserID}'";
                    StoreModel store = conn.Query<StoreModel>(query).AsList().FirstOrDefault();
                    return Ok(new { code = 200, store = store });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra " + ex.Message });
            }
        }

        public StoreModel _GetStoreByID(string StoreID = null)
        {
            using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
            {
                string query = $"SELECT * FROM Store WHERE id = '{StoreID}'";
                StoreModel store = conn.Query<StoreModel>(query).FirstOrDefault();

                string ratingQuery = $"SELECT r.* FROM Store s2 inner join Product p on s2.ID = p.StoreID inner join Cart c on p.ID = c.ProductID inner join Rating r on c.ID =r.CartID WHERE s2.ID = '{StoreID}'";
                List<RatingModel> ratings = conn.Query<RatingModel>(ratingQuery).AsList();

                float starSum = 0;
                foreach (var rating in ratings)
                {
                    starSum += rating.Star;
                }

                store.Ratings = ratings;
                store.Star = starSum / ratings.Count;

                return store;
            }
        }

        [HttpGet("GetStoreByID")]
        public IActionResult GetStoreByID(string StoreID = null)
        {
            try
            {
                return Ok(new
                {
                    code = 200,
                    store = _GetStoreByID(StoreID)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra " + ex.Message });
            }
        }

        [HttpGet("GetStoreByRange")]
        public IActionResult GetStoreByRange(int size, int page)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string query = $"SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY id) RowNr, * FROM Store ) t WHERE RowNr BETWEEN {page * size} AND {(page + 1) * size}";
                    List<StoreModel> store = conn.Query<StoreModel>(query).AsList();
                    return Ok(new { code = 200, store = store });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra " + ex.Message });
            }
        }

        [HttpPost("AddStoreByUser")]
        public IActionResult AddStoreByUser([FromBody] StoreModel Store)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string query = $"INSERT INTO Store (ID,Name,Detail,CreateTime,OwnerID,Approved) VALUES (N'{Guid.NewGuid()}', N'{Store.Name}', N'{Store.Detail}', '{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}', N'{Store.OwnerID}', 0); ";
                    conn.Execute(query);
                    return Ok(new { code = 200, message = "Thêm cửa hàng thành công" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra: " + ex.Message });
            }
        }
    }

}
