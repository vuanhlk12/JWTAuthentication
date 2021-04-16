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
    public class PaymentController : ControllerBase
    {
        [HttpGet("GetMethods")]
        public IActionResult GetAllMethod(string userID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string checkExist = $"SELECT * FROM AspNetUsers where Id = N'{userID}'";
                    string query = $"SELECT * FROM Payment where UserID = '{userID}'";
                    List<UserModel> users = conn.QueryAsync<UserModel>(checkExist).Result.AsList();
                    if(users.Count == 0)
                    {
                       return StatusCode(StatusCodes.Status404NotFound, new { code = 4041, message = "User không tồn tại" });
                    }
                    else
                    {
                        List<PaymentModel> methods = conn.QueryAsync<PaymentModel>(query).Result.AsList();
                        if (methods.Count == 0) return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Không có phương thức thanh toán" });
                        else return Ok(new { code = 200, message = methods });
                    }
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra", detail = e.Message });
            }
        }

        [HttpPost("AddMethod")]
        public IActionResult AddPayment(PaymentModel method)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string checkExistBanking = $"SELECT * FROM Payment where  CardNumber = N'{method.CardNumber}'";
                    string checkExistWallet = $"SELECT * FROM Payment where WalletID = N'{method.WalletID}'";
                    string query = $"INSERT INTO Payment(ID,Type,Detail,UserID,CardNumber,WalletID) VALUES(N'{Guid.NewGuid()}', N'{method.Type}', N'{method.Detail}', N'{method.UserID}', N'{method.CardNumber}', N'{method.WalletID}')";
                    List<PaymentModel> banking = conn.QueryAsync<PaymentModel>(checkExistBanking).Result.AsList();
                    List<PaymentModel> wallet = conn.QueryAsync<PaymentModel>(checkExistWallet).Result.AsList();
                    if (method.Type != "Banking" && method.Type != "Wallet")
                    {
                        return StatusCode(StatusCodes.Status405MethodNotAllowed, new { code = 405, message = "Thanh toán không hỗ trợ" });
                    }
                    else
                    {
                        if (method.Type == "Banking" && banking.Count > 0) return StatusCode(StatusCodes.Status409Conflict, new { code = 409, message = "Tài khoản ngân hàng đã tồn tại" });
                        else if (method.Type == "Wallet" && wallet.Count > 0) return StatusCode(StatusCodes.Status409Conflict, new { code = 409, message = "Ví điện tử đã tồn tại" });
                        else
                        {
                            conn.Execute(query);
                            return Ok(new { code = 200, message =  "Thêm phương thức thanh toán thành công" });
                        }
                    }
                  
                }
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra" , detail = e.Message });
            }
        }

        [HttpDelete("DeleteMethod")]
        public IActionResult DeletePayment(string paymentID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string checkExist = $"SELECT * FROM Payment where ID = N'{paymentID}'";
                  
                    string query = $"DELETE FROM Payment where ID = N'{paymentID}'";
                  
                    List<PaymentModel> wallet = conn.QueryAsync<PaymentModel>(checkExist).Result.AsList();
                    if(wallet.Count == 0) return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Phương thức thanh toán không tồn tại" });
                    else
                    {
                        conn.Execute(query);
                        return Ok(new { code = 200, message = "Xoá phương thức thanh toán thành công" });
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
