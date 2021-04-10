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
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra", detail  = e.Message });
            }
          
               
        }

        [HttpPost("AddFollow")]
        public IActionResult FollowStore(string userID, string storeID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string checkExist = $"SELECT * from [Following] where UserID = N'{userID}' and StoreID = N'{storeID}'";
                    string querry = $"INSERT INTO [Following](ID,UserID,StoreID,FollowTime) VALUES(N'{Guid.NewGuid()}', N'{userID}', N'{storeID}' , N'{DateTime.Now.ToString("yyyy-MM-dd h:mm")}')";
                    List<FollowingModel> result = conn.QueryAsync<FollowingModel>(checkExist).Result.AsList();
                    if(result.Count > 0) return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Người dùng đã theo dõi" });
                    else
                    {
                        conn.Execute(querry);
                        return Ok(new { code = 200, message = "Theo dõi thành công" });
                    }
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra", detail = e.Message });
            }
        }

        [HttpDelete("Unfollow")]
        public IActionResult UnfollowStore(string userID, string storeID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string checkExist = $"SELECT * from [Following] where UserID = N'{userID}' and StoreID = N'{storeID}'";
                    string querry = $"DELETE from [Following] where UserID = N'{userID}' and StoreID = N'{storeID}' ";
                    List<FollowingModel> result = conn.QueryAsync<FollowingModel>(checkExist).Result.AsList();
                    if (result.Count == 0) return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Không có người dùng này theo dõi" });
                    else
                    {
                        conn.Execute(querry);
                        return Ok(new { code = 200, message = "Bỏ theo dõi thành công" });
                    }
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra", detail = e.Message });
            }
        }

        [HttpGet("GetStoreFollowing")]
        public IActionResult GetStore(string userID)
        {
              try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string checkExist = $"SELECT * from AspNetUsers where Id = N'{userID}' ";
                    string querry = $"SELECT s.* from AspNetUsers u INNER JOIN [Following] f" +
                     $" on u.Id = f.UserID INNER JOIN Store s on s.ID = f.StoreID where u.Id = N'{userID}'";
                    List<UserModel> users = conn.QueryAsync<UserModel>(checkExist).Result.AsList();
                  
                    if (users.Count == 0) return StatusCode(StatusCodes.Status404NotFound, new { code = 4040, message = "Không tồn tài người dùng" });
                    else
                    {
                        List<StoreModel> stores = conn.QueryAsync<StoreModel>(querry).Result.AsList();
                        if(stores.Count == 0) return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Không tồn tài cửa hàng" });
                        return Ok(new { code = 200, message = stores });
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
