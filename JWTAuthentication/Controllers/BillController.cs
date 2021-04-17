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
    public class BillController : ControllerBase
    {
        [HttpGet("GetTransactions")]
        public IActionResult GetAllTransaction(string userID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string checkExist = $"SELECT * FROM AspNetUsers where Id = N'{userID}'";
                    string query = $"SELECT * FROM Bill where BuyerID = '{userID}'";
                    List<UserModel> users = conn.QueryAsync<UserModel>(checkExist).Result.AsList();
                    if (users.Count == 0)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "User không tồn tại" });
                    }
                    else
                    {
                        List<BillModel> methods = conn.QueryAsync<BillModel>(query).Result.AsList();
                        if (methods.Count == 0) return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Không có giao dich" });
                        else return Ok(new { code = 200, message = methods });
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
