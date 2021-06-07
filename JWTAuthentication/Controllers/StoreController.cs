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
using Microsoft.AspNetCore.Http;

namespace JWTAuthentication.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public StoreController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

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

                string ratingQuery = $"SELECT * FROM Rating r INNER JOIN Product p on r.ProductID =p.ID INNER JOIN Store s on p.StoreID =s.ID WHERE s.ID = '{StoreID}'";
                List<RatingModel> ratings = conn.Query<RatingModel>(ratingQuery).AsList();

                float starSum = 0;
                foreach (var rating in ratings)
                {
                    starSum += rating.Star;
                }

                store.Ratings = ratings;
                if (ratings.Count == 0)
                    store.Star = 0;
                else
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
        public IActionResult GetStoreByRange(int size, int page, int? aproveStatus = null, string searchKey = null)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string query = "FROM Store";
                    if (aproveStatus != null) query += $" where Approved ={aproveStatus}";
                    query = $"SELECT  * {query}";
                    var listAll = conn.Query<StoreModel>(query).AsList();

                    if (searchKey != null)
                    {
                        listAll = SearchByName(listAll, searchKey);
                    }

                    int count = listAll.Count();
                    var store = listAll.OrderBy(p => p.ID).Skip(size * page).Take(size).AsList();
                    return Ok(new { code = 200, total = count, store = store });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra " + ex.Message });
            }
        }

        public List<StoreModel> SearchByName(List<StoreModel> listAll, string searchKey)
        {
            string[] words = searchKey.Split(' ');
            List<StoreModel> returnList = new List<StoreModel>();
            foreach (StoreModel store in listAll)
            {
                int count = 0;
                foreach (string word in words)
                {
                    if (store.Name.ToLower().Contains(word.ToLower()))
                    {
                        count++;
                    }
                }
                if (count == words.Count())
                    returnList.Add(store);
            }
            return returnList;
        }

        [Authorize]
        [HttpPost("AddStoreByUser")]
        public async Task<IActionResult> AddStoreByUserAsync([FromBody] StoreModel Store)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string checkStore = $"SELECT * FROM Store WHERE OwnerID ='{user.Id}'";
                    var store = conn.Query<StoreModel>(checkStore).FirstOrDefault();
                    if (store != null) return StatusCode(StatusCodes.Status406NotAcceptable, new { code = 406, message = "User đã đăng ký cửa hàng" });

                    string query = $"INSERT INTO Store (ID,Name,Detail,CreateTime,OwnerID,Approved,RatingsCount,FollowerCount,Star) VALUES (N'{Guid.NewGuid()}', N'{Store.Name}', N'{Store.Detail}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', N'{user.Id}', 0,0,0,0); ";
                    conn.Execute(query);
                    return Ok(new { code = 200, message = "Thêm cửa hàng thành công" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra: " + ex.Message });
            }
        }

        [Authorize(Roles = UserRoles.Seller)]
        [HttpPost("UpdateStore")]
        public async Task<IActionResult> UpdateStore([FromBody] StoreModel Store)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string checkStore = $"SELECT * FROM Store WHERE OwnerID ='{user.Id}'";
                    var oldStore = conn.Query<StoreModel>(checkStore).FirstOrDefault();
                    if (oldStore.Approved != 1) return StatusCode(StatusCodes.Status406NotAcceptable, new { code = 406, message = "Cửa hàng chưa được chấp thuận hoặc đang bị khóa" });

                    string query = $"UPDATE Store SET Name=N'{Store.Name.Replace("'","''")}', Detail=N'{Store.Detail.Replace("'", "''")}' WHERE ID='{oldStore.ID}'";
                    conn.Execute(query);
                    return Ok(new { code = 200, message = $"Sửa cửa hàng {Store.Name} thành công" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra: " + ex.Message });
            }
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("ApproveStoreByAdmin")]
        public IActionResult ApproveStoreByAdmin(string StoreID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string query = $"UPDATE Store SET  Approved=1 WHERE ID='{StoreID}'";
                    conn.Execute(query);
                    return Ok(new { code = 200, message = "Xác thực cửa hàng thành công" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra: " + ex.Message });
            }
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("AproveStoreByAdmin")]
        public IActionResult AproveStoreByAdmin([FromBody] string StoreID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string query = $"UPDATE Store SET  Approved=1 WHERE ID='{StoreID}'";
                    conn.Execute(query);
                    return Ok(new { code = 200, message = "Xác thực cửa hàng thành công" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra: " + ex.Message });
            }
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("BanStoreByAdmin")]
        public IActionResult BanStoreByAdmin([FromBody] string StoreID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string query = $"UPDATE Store SET  Approved=2 WHERE ID='{StoreID}'";
                    conn.Execute(query);
                    return Ok(new { code = 200, message = "Cấm cửa hàng thành công" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra: " + ex.Message });
            }
        }
    }

}
