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
    public class FollowController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public FollowController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpGet("GetFollower")]
        public IActionResult GetStoreFollower(string storeID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string querry = $"SELECT u.* from AspNetUsers u INNER JOIN [Following] f" +
                    $" on u.Id = f.UserID INNER JOIN Store s on s.ID = f.StoreID where s.ID = N'{storeID}'";

                    List<UserModel> userList = conn.Query<UserModel>(querry).AsList();
                    if (userList.Count == 0)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Không có người theo dõi" });
                    }
                    else return Ok(new { code = 200, message = userList });

                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra", detail = e.Message });
            }


        }

        [Authorize]
        [HttpPost("AddFollow")]
        public async Task<IActionResult> FollowStoreAsync(string storeID, string userID = null)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string checkExist = $"SELECT * from [Following] where UserID = N'{user.Id}' and StoreID = N'{storeID}'";
                    string querry = $"INSERT INTO [Following](ID,UserID,StoreID,FollowTime) VALUES(N'{Guid.NewGuid()}', N'{user.Id}', N'{storeID}' , N'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')";
                    string addFollower = $"UPDATE Store SET FollowerCount = FollowerCount + 1 where ID = N'{storeID}'";
                    List<FollowingModel> result = conn.QueryAsync<FollowingModel>(checkExist).Result.AsList();
                    if (result.Count > 0) return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Người dùng đã theo dõi" });
                    else
                    {
                        conn.Execute(querry);
                        conn.Execute(addFollower);
                        return Ok(new { code = 200, message = "Theo dõi thành công" });
                    }
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra", detail = e.Message });
            }
        }

        [Authorize]
        [HttpDelete("Unfollow")]
        public async Task<IActionResult> UnfollowStoreAsync(string storeID, string userID = null)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string checkExist = $"SELECT * from [Following] where UserID = N'{user.Id}' and StoreID = N'{storeID}'";
                    string querry = $"DELETE from [Following] where UserID = N'{user.Id}' and StoreID = N'{storeID}' ";
                    string deleteFollower = $"UPDATE Store SET FollowerCount = FollowerCount - 1 where ID = N'{storeID}'";
                    List<FollowingModel> result = conn.QueryAsync<FollowingModel>(checkExist).Result.AsList();
                    if (result.Count == 0) return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Không có người dùng này theo dõi" });
                    else
                    {
                        conn.Execute(querry);
                        conn.Execute(deleteFollower);
                        return Ok(new { code = 200, message = "Bỏ theo dõi thành công" });
                    }
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra", detail = e.Message });
            }
        }

        [Authorize]
        [HttpGet("GetStoreFollowing")]
        public async Task<IActionResult> GetStoreAsync(string userID = null)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string checkExist = $"SELECT * from AspNetUsers where Id = N'{user.Id}' ";
                    string querry = $"SELECT s.* from AspNetUsers u INNER JOIN [Following] f" +
                     $" on u.Id = f.UserID INNER JOIN Store s on s.ID = f.StoreID where u.Id = N'{user.Id}'";
                    List<UserModel> users = conn.QueryAsync<UserModel>(checkExist).Result.AsList();

                    if (users.Count == 0) return StatusCode(StatusCodes.Status404NotFound, new { code = 4040, message = "Không tồn tài người dùng" });
                    else
                    {
                        List<StoreModel> stores = conn.QueryAsync<StoreModel>(querry).Result.AsList();
                        if (stores.Count == 0) return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Không có cửa hàng nào đang follow" });
                        return Ok(new { code = 200, total = stores.Count(), message = stores });
                    }
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra", detail = e.Message });
            }
        }
    }
}
