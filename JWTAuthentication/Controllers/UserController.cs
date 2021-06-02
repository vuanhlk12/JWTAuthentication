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
    public class UserController : ControllerBase
    {
        [HttpGet("GetUserByUserID")]
        public IActionResult GetUserByUserID(string UserID = null)
        {
            using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
            {
                try
                {
                    string query = $"SELECT * FROM AspNetUsers WHERE Id = '{UserID}'";
                    UserModel user = conn.Query<UserModel>(query).AsList().FirstOrDefault();
                    return Ok(new { code = 200, data = user });
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra " + ex.Message });
                }
            }
        }

        [HttpGet("GetUserByRange")]
        public IActionResult GetUserByRange(int page = 0, int size = 0, string UserName = null, string Email = null, string PhoneNumber = null, string Gender = null, string Name = null)
        {

            using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
            {
                try
                {
                    string query = $"FROM AspNetUsers";
                    query = $"SELECT  * {query}";
                    var listAll = conn.Query<UserModel>(query).AsList();

                    if (UserName != null) listAll = listAll.Where(p => p.UserName.ToUpper().Contains(UserName.ToUpper())).ToList();
                    if (PhoneNumber != null) listAll = listAll.Where(p => p.PhoneNumber.Contains(PhoneNumber)).ToList();
                    if (Email != null) listAll = listAll.Where(p => p.Email.ToUpper().Contains(Email.ToUpper())).ToList();
                    if (Gender != null) listAll = listAll.Where(p => p.Gender.ToUpper() == Gender.ToUpper()).ToList();
                    if (Name != null) listAll = listAll.Where(p => p.Name.ToUpper().Contains(Name.ToUpper())).ToList();

                    int count = listAll.Count();
                    if (count == 0) return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Không có kết quả ở trang này" });

                    var user = listAll.OrderBy(p => p.ID).Skip(size * page).Take(size).AsList();
                    return Ok(new { code = 200, total = count, data = user });
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra " + ex.Message });
                }
            }
        }

    }

}
